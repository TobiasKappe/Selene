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
    // Suboptimal implementation of a wizard in Windows.Forms
    // Why on earth does the toolkit not provide for this?

    public class WizardDialog<T> : NonModalPresenterBase<Forms.Control>
    {
        // Localisation?
        static readonly string NextText = "Next >";
        static readonly string PrevText = "< Previous";
        static readonly string CompleteText = "Complete";

        Form Win;
        int mCurrentIndex, MaxIndex;
        TableLayoutPanel ShiftingPanel, ButtonsPanel, MainPanel;
        Button NextButton, PrevButton;

        int CurrentIndex {
            get { return mCurrentIndex; }
            set
            {
                ShiftingPanel.Controls[mCurrentIndex].Visible = false;
                mCurrentIndex = value;
                ShiftingPanel.Controls[mCurrentIndex].Visible = true;

                PrevButton.Enabled = value != 0;
                NextButton.Text = value == MaxIndex ? CompleteText : NextText;
            }
        }

        Forms.Control CurrentPanel {
            get { return ShiftingPanel.Controls[mCurrentIndex]; }
        }

        public WizardDialog (string Title)
        {
            Win = new Form();
            Win.Text = Title;
            Win.AutoSize = true;
            Win.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Win.MaximizeBox = false;
            Win.FormBorderStyle = FormBorderStyle.FixedDialog;

            Win.Closed += WinClosed;
        }

        void WinClosed (object sender, EventArgs e)
        {
            Success = false;
        }

        void SizeChanged (object sender, EventArgs e)
        {
            MainPanel.Width = ShiftingPanel.Width = CurrentPanel.Width + 10;
            MainPanel.Height = CurrentPanel.Height + ButtonsPanel.Height + 20;
        }

        protected override void Build (ControlManifest Manifest)
        {
            MainPanel = new TableLayoutPanel();
            ButtonsPanel = new TableLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
            ShiftingPanel = new TableLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };

            ShiftingPanel.SizeChanged += SizeChanged;

            int i = 0;
            foreach(ControlCategory Cat in Manifest.Categories)
            {
                CatPanel Panel = new CatPanel(ProcureState);
                Panel.LayoutControls(State, Cat);
                ShiftingPanel.Controls.Add(Panel, i, 0);

                // Only display the first panel initially
                Panel.Visible = i < 1;

                i++;
            }
            MaxIndex = i-1;

            NextButton = new Button { Text = NextText };
            PrevButton = new Button { Text = PrevText };
            ButtonsPanel.Controls.Add(PrevButton, 0, 0);
            ButtonsPanel.Controls.Add(NextButton, 1, 0);

            MainPanel.Controls.Add(ShiftingPanel, 0, 1);
            MainPanel.Controls.Add(ButtonsPanel, 0, 2);

            NextButton.Click += NextButtonClick;
            PrevButton.Click += PrevButtonClick;

            Win.Controls.Add(MainPanel);

            CurrentIndex = 0;
        }

        void PrevButtonClick (object sender, EventArgs e)
        {
            if(CurrentIndex == 0) return;

            CurrentIndex--;
        }

        void NextButtonClick (object sender, EventArgs e)
        {
            if(CurrentIndex == MaxIndex)
            {
                Save();
                Success = true;

                Win.Visible = false;
                return;
            }

            CurrentIndex++;
        }

        public override void Show()
        {
            Win.Show();
        }

        public override void Hide()
        {
            Win.Hide();
        }

        protected override void Run()
        {
            Show();
        }

        public override void Block()
        {
            while(Done == null)
            {
                Application.DoEvents();
            }
        }
    }
}
