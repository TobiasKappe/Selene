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
using System.Windows.Forms;
using System.Collections.Generic;

using Selene.Backend;
using Selene.Winforms.Ordering;

using Forms = System.Windows.Forms;
using SB = Selene.Backend;

namespace Selene.Winforms.Frontend
{
    public class CatPanel : ControlVBox
    {
        public delegate IConverter<Forms.Control> ProcureState(SB.Control Cont);
        event ProcureState PState;

        public CatPanel (ProcureState State)
        {
            Location = new System.Drawing.Point(3,3);

            this.PState = State;
        }

        public void LayoutControls(List<IConverter<Forms.Control>> AddTo, ControlCategory Cat)
        {
            if(Cat.Subcategories.Length == 1)
            {
                Controls.Add(LayoutSubcat(AddTo, Cat.Subcategories[0]));

                return;
            }

            int CatIndex = 0;

            foreach(ControlSubcategory Subcat in Cat.Subcategories)
            {
                GroupBox SubcatBox = new GroupBox();
                SubcatBox.AutoSize = true;
                SubcatBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                SubcatBox.Text = Subcat.Name;

                TableLayoutPanel SubcatPanel = LayoutSubcat(AddTo, Subcat);

                SubcatBox.Controls.Add(SubcatPanel);
                Controls.Add(SubcatBox);
                CatIndex++;
            }
        }

        public TableLayoutPanel LayoutSubcat(List<IConverter<Forms.Control>> AddTo, ControlSubcategory Subcat)
        {
            TableLayoutPanel SubcatPanel = new TableLayoutPanel();
            SubcatPanel.AutoSize = true;
            SubcatPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            SubcatPanel.Location = new System.Drawing.Point(5,20);

            int SubcatIndex = 0;

            foreach(SB.Control Cont in Subcat.Controls)
            {
                IConverter<Forms.Control> Converter = PState(Cont);

                if(Converter != null)
                {
                    Forms.Control Widget = Converter.Construct(Cont);
                    Widget.AutoSize = true;

                    if(Cont.SubType != ControlType.Check)
                    {
                        Label L = new Label();
                        L.Text = Cont.Label;

                        SubcatPanel.Controls.Add(L, 1, SubcatIndex);
                        SubcatPanel.Controls.Add(Widget, 2, SubcatIndex++);
                    }
                    else SubcatPanel.Controls.Add(Widget, 1, SubcatIndex++);

                    AddTo.Add(Converter);
                }
            }

            return SubcatPanel;
        }
    }
}
