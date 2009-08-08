using System;
using System.Collections.Generic;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class FlagsChooser : FlagsBase<QObject>
    {
        QButtonGroup Group;
        QConverterProxy<Enum> Proxy;
        int i = 0;

        protected override IEnumerable<int> SelectedIndices {
            get
            {
                for(int p = 0; p < i; p++)
                {
                    if(Group.Button(p).Checked) yield return p;
                }
            }
        }

        protected override void ChangeIndex (int Index, bool Selected)
        {
            Group.Button(Index).Checked = Selected;
        }

        protected string ResolveType(ControlType Type)
        {
            return "buttonPressed(int)";
        }

        protected override QObject Construct ()
        {
            Proxy = new QConverterProxy<Enum>(Original, null);
            Proxy.Resolve = ResolveType;

            bool Vertical = false;
            Original.GetFlag<bool>(ref Vertical);

            QBoxLayout Lay;
            if(Vertical) Lay = new QVBoxLayout();
            else Lay = new QHBoxLayout();
            Group = new QButtonGroup(Lay);
            Proxy.Widg = Group;

            Group.Exclusive = false;

            return Lay;
        }

        protected override void AddOption (string Value)
        {
            QCheckBox Add = new QCheckBox(Value);
            Group.AddButton(Add, i++);
            (Widget as QBoxLayout).AddWidget(Add);
        }

        public override event EventHandler Changed {
            add { Proxy.Changed += value; }
            remove { Proxy.Changed -= value; }
        }
    }
}
