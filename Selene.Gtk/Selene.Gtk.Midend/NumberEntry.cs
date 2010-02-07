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
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class NumberEntry : ConverterBase<Widget, int>
    {
        protected override int ActualValue {
            get 
            { 
                if(Original.SubType == ControlType.Spin)
                    return (int) (Widget as SpinButton).Value; 
                else if(Original.SubType == ControlType.Glider)
                    return (int) (Widget as Scale).Value;
                else throw UnsupportedOverride();
            }
            set 
            {
                if(Original.SubType == ControlType.Spin)
                    (Widget as SpinButton).Value = (double) value;
                else if(Original.SubType == ControlType.Glider)
                    (Widget as Scale).Value = (double) value;    
                else throw UnsupportedOverride();
            }
        }
		
        protected override ControlType DefaultSubtype {
            get { return ControlType.Spin; }
        }
        
        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.Glider };
            }
        }

        protected override Widget Construct ()
        {
            int Min = int.MinValue, Max = int.MaxValue, Step = 1;
            bool Wrap = false;

            Original.GetFlag<int>(0, ref Min);
            Original.GetFlag(1, ref Max);
            Original.GetFlag(2, ref Step);
            Original.GetFlag(0, ref Wrap);

            if(Original.SubType == ControlType.Spin)
            {
                SpinButton Ret = new SpinButton((double)Min, (double)Max, (double) Step);
                Ret.Wrap = Wrap;
                return Ret;
            }
            else if(Original.SubType == ControlType.Glider)
            {
                bool Vertical = false;
                Original.GetFlag<bool>(0, ref Vertical);
                
                if(Vertical) 
                {
                    VScale Ret = new VScale((double)Min, (double)Max, (double)Step);
                    Ret.HeightRequest = 100;
                    return Ret;
                }
                else return new HScale((double)Min, (double)Max, (double)Step);
            }
            else throw UnsupportedOverride();
        }

        public override event EventHandler Changed {
            add 
            { 
                if(Original.SubType == ControlType.Spin)
                    (Widget as Entry).Changed += value; 
                else if(Original.SubType == ControlType.Glider)
                    (Widget as Scale).ValueChanged += value;
                else throw UnsupportedOverride();
            }
            remove
            {
                if(Original.SubType == ControlType.Spin)
                    (Widget as Entry).Changed -= value; 
                else if(Original.SubType == ControlType.Glider)
                    (Widget as Scale).ValueChanged -= value;                
                else throw UnsupportedOverride();
            }
        }
    }
}
