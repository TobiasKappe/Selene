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
