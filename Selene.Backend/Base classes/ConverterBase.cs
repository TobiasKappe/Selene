// Copyright (c) 2009 Tobias Kappé
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// Except as contained in this notice, the name(s) of the above
// copyright holders shall not be used in advertising or otherwise
// to promote the sale, use or other dealings in this Software
// without prior written authorization.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

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
