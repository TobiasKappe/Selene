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
            get { return (int) (Widget as SpinButton).Value; }
            set { (Widget as SpinButton).Value = (double)value; }
        }

        protected override Widget Construct ()
        {
            int Min = int.MinValue, Max = int.MaxValue, Step = 1;
            bool Wrap = false;

            Original.GetFlag<int>(0, ref Min);
            Original.GetFlag(1, ref Max);
            Original.GetFlag(2, ref Step);
            Original.GetFlag(0, ref Wrap);

            SpinButton Ret = new SpinButton((double)Min, (double)Max, (double) Step);
            Ret.Wrap = Wrap;

            return Ret;
        }

        public override event EventHandler Changed {
            add { (Widget as Entry).Changed += value; }
            remove { (Widget as Entry).Changed -= value; }
        }
    }
}