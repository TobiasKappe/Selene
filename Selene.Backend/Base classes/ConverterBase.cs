
using System;

namespace Selene.Backend
{
    public abstract class ConverterBase<WidgetType, TDisplay> : IConverter<WidgetType>
    {
        InvalidOperationException NoEventOverride;

        protected Control Original;
        protected WidgetType Widget;

        public Control Primitive {
            get { return Original; }
        }

        protected ConverterBase()
        {
            NoEventOverride = new InvalidOperationException("Type "+GetType()+" did not override Changed");
        }

        #region Interface implementation
        public Type Type { get { return typeof(TDisplay); } }

        public WidgetType Construct(Control Original)
        {
            this.Original = Original;
            int i;
            for(i = 0; i < Supported.Length; i++)
                if(Supported[i] == Original.SubType) break;

            if(i == Supported.Length && Original.SubType != ControlType.Default)
                throw new OverrideException(typeof(TDisplay), Original.SubType, Supported);

            return Widget = Construct();
        }

        public object Value {
            get { return (object) ActualValue; }
            set { ActualValue = (TDisplay) value; }
        }

        // It appears we can not have abstract events
        public virtual event EventHandler Changed {
            add { throw NoEventOverride; }
            remove { throw NoEventOverride; }
        }
        #endregion

        protected virtual ControlType[] Supported {
            get
            {
                return new ControlType[] { ControlType.Default };
            }
        }

        #region Abstract members
        protected abstract WidgetType Construct();
        protected abstract TDisplay ActualValue { get; set; }
        #endregion
    }
}
