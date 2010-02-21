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
using Selene.Winforms.Ordering;

namespace Selene.Winforms.Midend
{
    public class EnumChooser : EnumBase<Forms.Control>
    {
        EventHandler RadioProxy;
        bool Vertical;

        protected override ControlType DefaultSubtype {
            get { return ControlType.Dropdown; }
        }

        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.Radio };
            }
        }

        protected override int CurrentIndex {
            get
            {
                if(Original.SubType == ControlType.Radio)
                {
                    int i = 0;
                    foreach(RadioButton B in Widget.Controls)
                    {
                        if(B.Checked) return i;
                        i++;
                    }

                    return 0;
                }
                else if(Original.SubType == ControlType.Dropdown)
                    return (Widget as ComboBox).SelectedIndex;
                else throw UnsupportedOverride();
            }
            set
            {
                if(Original.SubType == ControlType.Radio)
                    (Widget.Controls[value] as RadioButton).Checked = true;
                else if(Original.SubType == ControlType.Dropdown)
                    (Widget as ComboBox).SelectedIndex = value;
                else throw UnsupportedOverride();
            }
        }

        protected override Forms.Control Construct ()
        {
            if(Original.SubType == ControlType.Radio)
            {
                Original.GetFlag<bool>(ref Vertical);
                return Vertical ? (Forms.Control) new ControlVBox() : (Forms.Control) new ControlHBox();
            }
            else if(Original.SubType == ControlType.Dropdown)
            {
                ComboBox Ret = new ComboBox();
                Ret.DropDownStyle = ComboBoxStyle.DropDownList;
                return Ret;
            }
            else throw UnsupportedOverride();
        }

        public override event EventHandler Changed {
            add
            {
                if(Original.SubType == ControlType.Radio) RadioProxy += value;
                else if(Original.SubType == ControlType.Dropdown)
                    (Widget as ComboBox).SelectedIndexChanged += value;
                else throw UnsupportedOverride();
            }
            remove
            {
                if(Original.SubType == ControlType.Radio) RadioProxy -= value;
                else if(Original.SubType == ControlType.Dropdown)
                    (Widget as ComboBox).SelectedIndexChanged -= value;
                else throw UnsupportedOverride();
            }
        }

        protected override void AddOption (string Value)
        {
            if(Original.SubType == ControlType.Radio)
            {
                RadioButton Add = new RadioButton();
                Add.CheckedChanged += RadioCheckedChanged;
                Add.Text = Value;

                Widget.Controls.Add(Add);
            }
            else (Widget as ComboBox).Items.Add(Value);
        }

        void RadioCheckedChanged (object sender, EventArgs e)
        {
            if((sender as RadioButton).Checked && RadioProxy != null)
                RadioProxy((sender as RadioButton).Parent, e);
        }
    }
}
