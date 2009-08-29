using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class NumberEntry : QConverterProxy<int>
    {
        protected override int ActualValue {
            get { return (Widget as QSpinBox).Value; }
            set { (Widget as QSpinBox).Value = value; }
        }

        protected override QObject Construct ()
        {
            int Min = int.MinValue, Max = int.MaxValue, Step = 1;
            bool Wrap = false;

            Original.GetFlag<int>(0, ref Min);
            Original.GetFlag(1, ref Max);
            Original.GetFlag(2, ref Step);
            Original.GetFlag(0, ref Wrap);

            QSpinBox Ret = new QSpinBox();
            Ret.Maximum = Max;
            Ret.Minimum = Min;
            Ret.Wrapping = Wrap;
            Ret.StepBy(Step);

            return Ret;
        }

        protected override string SignalForType (ControlType Type)
        {
            return "valueChanged(int)";
        }
    }
}
