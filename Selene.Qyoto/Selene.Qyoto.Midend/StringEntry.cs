using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class StringEntry : QConverterProxy<string>
    {
        protected override string ActualValue {
            get
            {
                return (Widget as QLineEdit).Text;
            }
            set
            {
                (Widget as QLineEdit).Text = value;
            }
        }

        protected override ControlType[] Supported {
            get
            {
                // We do not "support" these modes but default to entry for all
                return new ControlType[] { ControlType.Entry, ControlType.FileSelect, ControlType.DirectorySelect };
            }
        }

        protected override QObject Construct ()
        {
            return new QLineEdit();
        }

        protected override string SignalForType (ControlType Type)
        {
            return "textChanged(QString)";
        }
    }
}
