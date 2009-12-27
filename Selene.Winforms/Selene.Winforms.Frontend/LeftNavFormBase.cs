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

namespace Selene.Winforms.Frontend
{
    // Base class for ListStoreDialog and TreeStoreDialog in the Windows frontend
    public abstract class LeftNavFormBase<T> : ModalFormBase<T>
    {
        int MaxWidth = 0, MaxHeight;
        protected TableLayoutPanel Panel;
        protected Control ActivePanel;

        public LeftNavFormBase (string Title) : base(Title)
        {
        }

        protected override Control ActualWidget {
            get { return Panel; }
        }

        protected void RightPanelResized (object sender, EventArgs e)
        {
            Control Cont = sender as Control;

            if(Cont.Width > MaxWidth)
            {
                Win.AutoSize = false;
                MaxWidth = Cont.Width;
                Win.Width = MaxWidth + Navigation.Width + 20;
            }

            if(Cont.Height > MaxHeight)
            {
                // Maybe the navigation widget is bigger
                MaxHeight = (Cont.Height > Navigation.Height ? Cont.Height : Navigation.Height);
                Win.Height = MaxHeight + 70; // Take button height into account
            }
        }

        protected sealed override void Build (Selene.Backend.ControlManifest Manifest)
        {
            int Column = 2;
            Panel = new TableLayoutPanel();
            Panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            Build(Manifest, Column);

            Panel.Controls.Add(Navigation, 1, 1);
            Panel.Location = new System.Drawing.Point(3,3);
            Panel.AutoSize = true;
            MainBox.Controls.Add(Panel);

            base.Build(Manifest);
        }

        protected abstract void Build(Selene.Backend.ControlManifest Manifest, int Column);

        protected abstract Control Navigation
        {
            get;
        }
    }
}
