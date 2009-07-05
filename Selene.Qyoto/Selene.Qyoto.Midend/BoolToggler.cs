using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class BoolToggler : QConverterProxy<bool>
    {
        protected override bool ActualValue {
            get
            {
                return (Widget as QCheckBox).Checked;
            }
            set
            {
                (Widget as QCheckBox).Checked = value;
            }
        }

        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.Default, ControlType.Check };
            }
        }

        protected override QObject Construct ()
        {
            Original.SubType = ControlType.Check;
            return new QCheckBox(Original.Label);
        }

        protected override string SignalForType (ControlType Type)
        {
            return "toggled(bool)";
        }
    }
}
