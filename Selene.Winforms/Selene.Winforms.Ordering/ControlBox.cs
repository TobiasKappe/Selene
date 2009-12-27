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
using System.Drawing;

namespace Selene.Winforms.Ordering
{
    public abstract class ControlBox : UserControl, IControlBox
    {
        public static readonly int DefaultSpacing = 5;
        public static readonly int DefaultPadding = 0;

        protected int mSpacing;
        protected int mPadding;

        protected int x;
        protected int y;

        public int Spacing {
            get { return mSpacing; }
            set
            {
                mSpacing = value;
                LayoutIfVisible();
            }
        }

        public new int Padding {
            get { return mPadding; }
            set
            {
                mPadding = value;
                LayoutIfVisible();
            }
        }

        public ControlBox() : this(DefaultSpacing, DefaultPadding)
        {
        }

        public ControlBox(int Spacing, int Padding)
        {
            mSpacing = Spacing;
            mPadding = Padding;

            Width = Height = 0;
        }

        protected override void OnVisibleChanged (System.EventArgs e)
        {
            base.OnVisibleChanged (e);
            LayoutIfVisible();
        }

        private void LayoutIfVisible()
        {
            if(Visible) LayoutControls();
        }

        private void LayoutIfVisible(object sender, EventArgs e)
        {
            Control Sent = sender as Control;
            if(Sent == null) return;

            if(Sent.Visible)
                LayoutIfVisible();
        }

        protected override void OnControlAdded (ControlEventArgs e)
        {
            if(!(e.Control is ControlBox))
                e.Control.AutoSize = true;

            e.Control.SizeChanged += LayoutIfVisible;
            LayoutIfVisible();
        }

        protected override void OnControlRemoved (System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlRemoved (e);

            e.Control.SizeChanged -= LayoutIfVisible;
            LayoutIfVisible();
        }

        protected void LayoutControls()
        {
            x = Padding;
            y = Padding;

            int MaxSecondary = 0;
            int PrimarySum = 0;
            int i = 0;

            foreach(Control Cont in Controls)
            {
                int PrimaryTrait, SecondaryTrait;
                TraitsFor(Cont, out PrimaryTrait, out SecondaryTrait);

                if(SecondaryTrait > MaxSecondary)
                    MaxSecondary = SecondaryTrait;

                Cont.Location = new Point(x, y);

                PrimarySum += PrimaryTrait;
                i++;

                Shift(Cont);
            }

            SecondaryDimension = Padding + MaxSecondary + Padding;
            PrimaryDimension = Padding + PrimarySum + (i-1)*Spacing + Padding;
        }

        protected abstract int PrimaryDimension {
            get; set;
        }

        protected abstract int SecondaryDimension {
            get; set;
        }

        protected abstract void TraitsFor(Control Subject, out int Primary, out int Secondary);

        protected abstract void Shift(Control Subject);
    }
}