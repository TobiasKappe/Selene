using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class EnumChooser : EnumBase<QObject>
    {
        QButtonGroup Group;
        QConverterProxy<Enum> Proxy;
        int i = 0;

        protected override ControlType[] Supported {
            get
            {
                return new ControlType[] { ControlType.Default, ControlType.Dropdown, ControlType.Radio };
            }
        }

        protected override int CurrentIndex {
            get
            {
                if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
                    return (Widget as QComboBox).CurrentIndex;
                else if(Original.SubType == ControlType.Radio)
                    return ((Widget as QBoxLayout).Children()[0] as QButtonGroup).CheckedId();

                return 0;
            }
            set
            {
                if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
                    (Widget as QComboBox).CurrentIndex = value;
                else if(Original.SubType == ControlType.Radio)
                    Group.Button(value).Checked = true;
            }
        }

        protected string ResolveType(ControlType Type)
        {
            if(Type == ControlType.Default || Type == ControlType.Dropdown) return "activated(int)";
            else if(Type == ControlType.Radio) return "buttonPressed(int)";
            else return string.Empty;
        }

        protected override QObject Construct ()
        {
            Proxy = new QConverterProxy<Enum>(Original, null);
            Proxy.Resolve = ResolveType;

            if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
                return Proxy.Widg = new QComboBox();
            else if(Original.SubType == ControlType.Radio)
            {
                bool Vertical = false;
                Original.GetFlag<bool>(ref Vertical);

                QBoxLayout Lay;
                if(Vertical) Lay = new QVBoxLayout();
                else Lay = new QHBoxLayout();
                Group = new QButtonGroup(Lay);
                Proxy.Widg = Group;

                return Lay;
            }

            return null;
        }

        protected override void AddOption (string Value)
        {
            if(Original.SubType == ControlType.Default || Original.SubType == ControlType.Dropdown)
                (Widget as QComboBox).AddItem(Value);
            else if(Original.SubType == ControlType.Radio)
            {
                QRadioButton Add = new QRadioButton(Value);
                Group.AddButton(Add, i++);
                (Widget as QBoxLayout).AddWidget(Add);
            }
        }

        public override event EventHandler Changed {
            add { Proxy.Changed += value; }
            remove { Proxy.Changed -= value; }
        }
    }
}
