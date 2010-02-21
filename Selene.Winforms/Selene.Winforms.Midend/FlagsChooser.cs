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
        bool Vertical = false;
        int Pos = 0;
        EventHandler Proxy;

        protected override IEnumerable<int> SelectedIndices {
            get
            {
                int i = 0;
                foreach(CheckBox Box in Boxes.Controls)
                {
                    if(Box.Checked) yield return i;
                    i++;
                }
            }
        }

        protected override Forms.Control Construct ()
        {
            Original.GetFlag<bool>(ref Vertical);

            Boxes = new TableLayoutPanel();
            Boxes.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            return Boxes;
        }

        protected override void AddOption (string Value)
        {
            CheckBox Add = new CheckBox();
            Add.Text = Value;
            Add.CheckedChanged += HandleChange;
            Add.AutoSize = true;

            if(Vertical) Boxes.Controls.Add(Add, 0, Pos++);
            else Boxes.Controls.Add(Add, Pos++, 0);
        }

        protected override void ChangeIndex (int Index, bool Selected)
        {
            (Boxes.Controls[Index] as CheckBox).Checked = Selected;
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
