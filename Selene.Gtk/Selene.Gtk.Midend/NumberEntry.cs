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
