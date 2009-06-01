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
		protected Control[] State;
		
		public event Done OnDone;
		bool HasBuilt = false;
		
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
		
		protected Control ProcureState(Control Original)
		{
			object Pass = Original.Info.GetValue(Present);
			
			foreach(IConverter Converter in Converters)
			{				
				if(Converter.Type == Original.Type || (Original.Type.IsEnum && Converter.Type == typeof(Enum))) 
				{
					Control Add = Converter.ToWidget(Original, Pass);
					Add.Converter = Converter;
					return Add;
				}
			}
			
			if(Original.Type.IsArray && ArrayConverter != null && !Original.Type.GetElementType().IsValueType)
			{
				IConverter Viewer = (IConverter)ArrayConverter.Invoke(null);
				Viewer.Type = Original.Type.GetElementType();
				Original = Viewer.ToWidget(Original, Pass);
				Original.Converter = Viewer;
			
				return Original;
			}	
			
			return null;
		}
		
		#region Partial interface implementation
		public virtual bool Run(SaveType Present)
		{
			Prepare(Present);
			Show();

			return true;
		}

		public void Save()
		{
            Save(Present);
		}

        public void Save(SaveType To)
        {
            foreach(Control Cont in State)
            {
                if(Cont.Converter == null) continue;
                Cont.Info.SetValue(To, Cont.Converter.ToObject(Cont));
            }
        }
		#endregion

		private void SetFields()
		{
            foreach(Control Cont in State)
            {
                object Pass = Cont.Info.GetValue(Present);
                Cont.Converter.SetValue(Cont, Pass);
            }
		}
		
		protected void Prepare(SaveType Present)
		{
			this.Present = Present;	

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
