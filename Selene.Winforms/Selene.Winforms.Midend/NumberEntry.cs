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
            get { return (int) (Widget as NumericUpDown).Value; }
            set { (Widget as NumericUpDown).Value = value; }
        }

        protected override Forms.Control Construct ()
        {
            NumericUpDown Ret = new NumericUpDown();

            int Min = int.MinValue, Max = int.MaxValue, Step = 1;
            bool Wrap = false;

            Original.GetFlag<int>(0, ref Min);
            Original.GetFlag(1, ref Max);
            Original.GetFlag(2, ref Step);
            Original.GetFlag(0, ref Wrap);

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
            add { (Widget as NumericUpDown).ValueChanged += value; }
            remove { (Widget as NumericUpDown).ValueChanged -= value; }
        }
    }
}
