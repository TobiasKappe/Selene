
using System;

namespace Selene.Backend
{
	public abstract class ConverterBase<TController, TDisplay> : IConverter where TController: Control, new()
	{
		#region Interface implementation
		public Type Type { get { return typeof(TDisplay); } set { }}
		
		public Control ToWidget(Control Original, object Value)
		{
			TController Orig = new TController();
			Orig.Label = Original.Label;
			Orig.Type = Original.Type;
			Orig.SubType = Original.SubType;
			Orig.Info = Original.Info;
			Orig.Name = Original.Name;
			Orig.Flags = Original.Flags;
			
			return ToWidget(Orig, (TDisplay) Value);
		}
		
		public object ToObject(Control Start)
		{
			return ToValue(Start as TController);
		}
		
		public void SetValue(Control Original, object Start)
		{
			SetValue((TController) Original, (TDisplay)Start);
		}
		#endregion
		
		#region Abstract members
		protected abstract TController ToWidget(TController Original, TDisplay Value);
		protected abstract TDisplay ToValue(TController Start);
		protected abstract void SetValue(TController Original, TDisplay Value);
		#endregion
	}
}
