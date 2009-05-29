using System;
using System.Collections.Generic;
using System.Reflection;

namespace Selene.Backend
{
	public delegate void Done();
	
	public abstract class DisplayBase <SaveType> : IPresenter<SaveType> where SaveType : class
	{ 
		protected static ControlManifest Manifest;
		protected static IConverter[] Converters;
		protected static ConstructorInfo ArrayConverter;
		protected SaveType Present;
		private bool HasBuilt = false;
		public event Done OnDone;
		
		static DisplayBase()
		{
			Assembly Caller = Assembly.GetCallingAssembly();
			Converters = Introspector.GetConverters(Caller, out ArrayConverter);
			
			if(Converters.Length < 1) WarningFactory.Warn("No converters found in assembly "+Caller.FullName+". Presenters will be empty");
			
			if(typeof(SaveType) != typeof(object))
				Manifest = Introspector.Inspect(typeof(SaveType));
		}
		
		internal void ForceInspect(Type Inspect)
		{
			Manifest = Introspector.Inspect(Inspect);
			ResetConverters();
		}
		
		private void ResetConverters()
		{
			Manifest.EachControl(delegate(ref Control Arg) {
				Arg.Converter = null;
			});
		}
		
		public static void StubManifest(string Filename)
		{
			Manifest.Save(Filename);
		}
		
		protected DisplayBase()
		{
			if(Manifest != null) ResetConverters();
		}
		
		#region Partial interface implementation
		public virtual bool Run(SaveType Present)
		{			
			Prepare(Present);
			Show();
			Save();
			
			return true;
		}
		
		public void Save()
		{
			Manifest.EachControl(SaveField);
		}
		#endregion

		#region Iterating functions
		private void SaveField(ref Control Cont)
		{
			if(Cont.Converter == null) return;
			
			Cont.Info.SetValue(Present, Cont.Converter.ToObject(Cont));
		}
		
		private void SetField(ref Control Cont)
		{	
			object Pass = Cont.Info.GetValue(Present);
			
			if(Cont.Converter != null)
			{
				Cont.Converter.SetValue(Cont, Pass);
				return;
			}
			
			foreach(IConverter Converter in Converters)
			{				
				if(Converter.Type == Cont.Type || (Cont.Type.IsEnum && Converter.Type == typeof(Enum))) 
				{
					Cont = Converter.ToWidget(Cont, Pass);
					Cont.Converter = Converter;
					return;
				}
			}
			
			if(Cont.Type.IsArray && !Cont.Type.GetElementType().IsValueType && ArrayConverter != null)
			{
				IConverter Viewer = (IConverter)ArrayConverter.Invoke(null);
				Viewer.Type = Cont.Type.GetElementType();
				Cont = Viewer.ToWidget(Cont, Pass);
				Cont.Converter = Viewer;
			}
		}
		#endregion
		
		private void SetFields()
		{
			Manifest.EachControl(SetField);
		}
		
		protected void Prepare(SaveType Present)
		{
			this.Present = Present;	
			SetFields();
			if(!HasBuilt)
			{
				Build();
				HasBuilt = true;
			}
		}
		
		protected void FireDone()
		{
			if(OnDone != null) OnDone();
		}
		
		#region Abstract members
		protected abstract void Build();
		public abstract void Hide();
		public abstract void Show();
		#endregion
	}
}
