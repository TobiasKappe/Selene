
using System;

namespace Selene.Backend
{
    public abstract class ConverterBase<TController, TDisplay> : IConverter where TController: Control, new()
    {
        #region Interface implementation
        public Type Type { get { return typeof(TDisplay); } }
        protected ControlType[] Supported = new ControlType[] { ControlType.Default };
        
        // To prevent implicit base() invocation from setting Supported to a zero-length array
        protected ConverterBase()
        {
        }
        
        protected ConverterBase(params ControlType[] Supported)
        {
            this.Supported = Supported;
        }
        
        public Control ToWidget(Control Original)
        {
            int i;
            for(i = 0; i < Supported.Length; i++)
                if(Supported[i] == Original.SubType) break;
            
            if(i == Supported.Length && Original.SubType != ControlType.Default) 
                throw new OverrideException(typeof(TDisplay), Original.SubType, Supported);
            
            TController Orig = new TController();
            Orig.Label = Original.Label;
            Orig.SubType = Original.SubType;
            Orig.Info = Original.Info;
            Orig.Flags = Original.Flags;
            
            return ToWidget(Orig);
        }
        
        public object ToObject(Control Start)
        {
            return ToValue(Start as TController);
        }
        
        public void SetValue(Control Original, object Start)
        {
            SetValue((TController) Original, (TDisplay)Start);
        }

        public void ConnectChange(Control Original, EventHandler OnChange)
        {
            ConnectChange((TController)Original, OnChange);
        }
        #endregion

        #region Abstract members
        protected abstract void ConnectChange(TController Original, EventHandler OnChange);
        protected abstract TController ToWidget(TController Original);
        protected abstract TDisplay ToValue(TController Start);
        protected abstract void SetValue(TController Original, TDisplay Value);
        #endregion
    }
}
