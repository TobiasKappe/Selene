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

namespace Selene.Winforms.Frontend
{
    public class ListStoreDialog<T> : ModalFormBase
    {
        ListBox List;
        TableLayoutPanel Panel;

        public ListStoreDialog (string Title) : base(Title)
        {
        }

        protected override void Build (ControlManifest Manifest)
        {
            int Column = 1;
            Panel = new TableLayoutPanel();
            Panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            List = new ListBox();
            Panel.Controls.Add(List, Column++, 1);
            Panel.Location = new System.Drawing.Point(3,3);

            List.BeginUpdate();
            foreach(ControlCategory Cat in Manifest.Categories)
            {
                List.Items.Add(Cat.Name);
                CatPanel SubPanel = new CatPanel(ProcureState);
                SubPanel.LayoutControls(State, Cat);

                Panel.Controls.Add(SubPanel, Column++, 1);

                if(Column != 2)
                    SubPanel.Visible = false;
            }
            List.EndUpdate();

            // TODO: Add buttons!

            List.SelectedIndexChanged += ListSelectedIndexChanged;

            // TODO: Autosizing is not done correctly here
            Panel.AutoSize = true;
            Win.Controls.Add(Panel);
        }

        void ListSelectedIndexChanged (object sender, EventArgs e)
        {
            for(int i = 1; i < Panel.Controls.Count; i++)
                Panel.Controls[i].Visible = i-1 == List.SelectedIndex;
        }
    }
}
