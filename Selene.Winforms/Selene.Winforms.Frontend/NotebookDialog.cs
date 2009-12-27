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
    public class NotebookDialog<T> : ModalFormBase<T>
    {
        TabControl Tabbed;

        public NotebookDialog(string Title) : base(Title)
        {
        }

        protected override Forms.Control ActualWidget {
            get { return Tabbed; }
        }

        void CatPanelResize(object Sender, EventArgs e)
        {
            var Set = new System.Drawing.Size();

            foreach(Forms.Control Tab in Tabbed.Controls)
            {
                var Size = Tab.Controls[0].Size;

                // Correct for the tab bar and padding
                Size.Width += 10;
                Size.Height += 30;

                if(Size.Width > Set.Width) Set.Width = Size.Width;
                if(Size.Height > Set.Height) Set.Height = Size.Height;
            }

            Tabbed.Size = Set;
        }

        protected override void Build (ControlManifest Manifest)
        {
            if(Manifest.Categories.Length == 1)
            {
                CatPanel Panel = new CatPanel(ProcureState);
                Panel.LayoutControls(State, Manifest.Categories[0]);
                MainBox.Controls.Add(Panel);

                base.Build(Manifest);
                return;
            }

            Tabbed = new TabControl();
            Tabbed.AutoSize = true;

            foreach(ControlCategory Cat in Manifest.Categories)
            {
                TabPage Page = new TabPage();
                Page.Text = Cat.Name;
                Page.AutoSize = true;
                Page.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Tabbed.Controls.Add(Page);

                CatPanel Panel = new CatPanel(ProcureState);

                Page.Controls.Add(Panel);
                // Somehow the tab control can not autosize
                Panel.SizeChanged += CatPanelResize;

                Panel.LayoutControls(State, Cat);
            }

            MainBox.Controls.Add(Tabbed);

            base.Build(Manifest);
        }
    }
}
