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
using SB = Selene.Backend;
using System.Windows.Forms;
using Forms = System.Windows.Forms;
using System.Collections.Generic;

namespace Selene.Winforms.Frontend
{
    public class CatPanel : TableLayoutPanel
    {
        public delegate IConverter<Forms.Control> ProcureState(SB.Control Cont);
        event ProcureState PState;

        public CatPanel (ProcureState State)
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Location = new System.Drawing.Point(3,3);


            this.PState = State;
        }

        public void LayoutControls(List<IConverter<Forms.Control>> AddTo, ControlCategory Cat)
        {
            int CatIndex = 0;

            foreach(ControlSubcategory Subcat in Cat.Subcategories)
            {
                GroupBox SubcatBox = new GroupBox();
                SubcatBox.AutoSize = true;
                SubcatBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                TableLayoutPanel SubcatPanel = new TableLayoutPanel();
                SubcatPanel.AutoSize = true;
                SubcatPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                Controls.Add(SubcatBox, CatIndex, 1);
                SubcatBox.Controls.Add(SubcatPanel);

                SubcatPanel.Location = new System.Drawing.Point(5,20);
                SubcatBox.Text = Subcat.Name;

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
            }
        }

    }
}
