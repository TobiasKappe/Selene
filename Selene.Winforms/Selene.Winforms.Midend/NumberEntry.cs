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
using Forms = System.Windows.Forms;
using System.Windows.Forms;
using Selene.Backend;

namespace Selene.Winforms.Midend
{
    public class NumberEntry : ConverterBase<Forms.Control, int>
    {
        protected override int ActualValue {
            get 
            { 
                if(Original.SubType == ControlType.Spin)
                    return (int) (Widget as NumericUpDown).Value;
                else if(Original.SubType == ControlType.Glider)
                    return (int) (Widget as TrackBar).Value;
                else throw UnsupportedOverride();
            }
            set 
            {
                if(Original.SubType == ControlType.Spin)
                    (Widget as NumericUpDown).Value = value;
                else if(Original.SubType == ControlType.Glider)
                    (Widget as TrackBar).Value = value;
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

        protected override Forms.Control Construct ()
        {
            int Min = int.MinValue, Max = int.MaxValue, Step = 1;

            Original.GetFlag<int>(0, ref Min);
            Original.GetFlag(1, ref Max);
            Original.GetFlag(2, ref Step);
            
            if(Original.SubType == ControlType.Spin)
            {
                bool Wrap = false;
                Original.GetFlag(0, ref Wrap);
                
                NumericUpDown Ret = new NumericUpDown();
                Ret.Maximum = Max;
                Ret.Minimum = Min;
                Ret.Increment = Step;
    
                if(Wrap)
                {
                    Ret.ValueChanged += ValueChanged;
    
                    // We're safe from overflowing here, because
                    // int.MaxValue < decimal.MaxValue and int.MinValue > decimal.MinValue
                    // and the last value for these came from ints.
                    Ret.Maximum++;
                    Ret.Minimum--;
                }
                return Ret;
            }
            else if(Original.SubType == ControlType.Glider)
            {
                TrackBar Ret = new TrackBar();
                Ret.Maximum = Max;
                Ret.Minimum = Min;
                Ret.SmallChange = Step;
                Ret.LargeChange = Step*10;
                Ret.TickStyle = TickStyle.None;
                
                bool Vertical = false;
                Original.GetFlag<bool>(0, ref Vertical);
                if(Vertical)
                    Ret.Orientation = Orientation.Vertical;
                
                return Ret;
            }
            else throw UnsupportedOverride();
        }

        // Simulate wrapping
        void ValueChanged (object sender, EventArgs e)
        {
            var UpDown = sender as NumericUpDown;

            if(UpDown.Value == UpDown.Maximum)
                UpDown.Value = UpDown.Minimum+1;
            else if(UpDown.Value == UpDown.Minimum)
                UpDown.Value = UpDown.Maximum-1;
        }

        public override event EventHandler Changed {
            add 
            { 
                if(Original.SubType == ControlType.Spin)
                    (Widget as NumericUpDown).ValueChanged += value; 
                else if(Original.SubType == ControlType.Glider)
                    (Widget as TrackBar).ValueChanged += value;
                else throw UnsupportedOverride();
            }
            remove
            {
                if(Original.SubType == ControlType.Spin)
                    (Widget as NumericUpDown).ValueChanged -= value; 
                else if(Original.SubType == ControlType.Glider)
                    (Widget as TrackBar).ValueChanged -= value;                
                else throw UnsupportedOverride();
            }
        }
    }
}
