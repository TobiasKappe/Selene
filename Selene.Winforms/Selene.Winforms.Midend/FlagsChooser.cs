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
using System.Collections.Generic;
using Selene.Backend;

namespace Selene.Winforms.Midend
{
    public class FlagsChooser : FlagsBase<Forms.Control>
    {
        TableLayoutPanel Boxes;
        ListView List;
        
        bool Vertical = false;
        int Pos = 0;
        EventHandler Proxy;
        
        protected override ControlType DefaultSubtype {
            get { return ControlType.MultiCheck; }
        }
        
        protected override ControlType[] Supported {
            get { return new ControlType[] { ControlType.MultiSelect }; }
        }

        protected override IEnumerable<int> SelectedIndices {
            get
            {
                if(Original.SubType == ControlType.MultiCheck)
                {
                    int i = 0;
                    foreach(CheckBox Box in Boxes.Controls)
                    {
                        if(Box.Checked) yield return i;
                        i++;
                    }
                }
                else if(Original.SubType == ControlType.MultiSelect)
                {
                    // For some reason we can not return the whole list at once
                    foreach(int i in List.SelectedIndices)
                        yield return i;
                }
                else throw UnsupportedOverride();
            }
        }

        protected override Forms.Control Construct ()
        {
            if(Original.SubType == ControlType.MultiCheck)
            {
                Original.GetFlag<bool>(ref Vertical);
    
                Boxes = new TableLayoutPanel();
                Boxes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    
                return Boxes;
            }
            else if(Original.SubType == ControlType.MultiSelect)
            {
                List = new ListView();
                List.Columns.Add("Option");
                List.SelectedIndexChanged += HandleChange;
                List.FullRowSelect = true;
                List.AllowColumnReorder = false;
                List.View = View.Details;
                List.HeaderStyle = ColumnHeaderStyle.None;
                List.MultiSelect = true;
                return List;
            }
            else throw UnsupportedOverride();
        }

        protected override void AddOption (string Value)
        {
            if(Original.SubType == ControlType.MultiCheck)
            {
                CheckBox Add = new CheckBox();
                Add.Text = Value;
                Add.CheckedChanged += HandleChange;
                Add.AutoSize = true;
    
                if(Vertical) Boxes.Controls.Add(Add, 0, Pos++);
                else Boxes.Controls.Add(Add, Pos++, 0);
            }
            else if(Original.SubType == ControlType.MultiSelect)
                List.Items.Add(Value);
            else throw UnsupportedOverride();
        }

        protected override void ChangeIndex (int Index, bool Selected)
        {
            if(Original.SubType == ControlType.MultiCheck)
                (Boxes.Controls[Index] as CheckBox).Checked = Selected;
            else if(Original.SubType == ControlType.MultiSelect)
                List.Items[Index].Selected = Selected;
            else throw UnsupportedOverride();
        }

        void HandleChange(object sender, EventArgs args)
        {
            if(Proxy != null)
                Proxy(Boxes, args);
        }

        public override event EventHandler Changed {
            add { Proxy += value; }
            remove { Proxy -= value; }
        }
    }
}
