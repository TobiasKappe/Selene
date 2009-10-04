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
    public class NotebookDialog<T> : ModalPresenterBase<Forms.Control>
    {
        Form Win;
        TabControl Tabbed;

        public NotebookDialog(string Title)
        {
            Win = new Form();

            Win.Text = Title;
            Win.MaximizeBox = false;
            Win.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        protected override void Build (ControlManifest Manifest)
        {
            Tabbed = new TabControl();
            Tabbed.Location = new System.Drawing.Point(5, 5);

            foreach(ControlCategory Cat in Manifest.Categories)
            {
                TabPage Page = new TabPage();
                Page.Text = Cat.Name;
                Page.AutoSize = true;
                Page.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                TableLayoutPanel CatPanel = new TableLayoutPanel();
                CatPanel.AutoSize = true;
                CatPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                Page.Controls.Add(CatPanel);

                int CatIndex = 0;

                foreach(ControlSubcategory Subcat in Cat.Subcategories)
                {
                    Label SubcatLabel = new Label();
                    SubcatLabel.Text = Subcat.Name;
                    SubcatLabel.Font = new System.Drawing.Font(SubcatLabel.Font, System.Drawing.FontStyle.Bold);
                    CatPanel.Controls.Add(SubcatLabel, 1, CatIndex++);

                    foreach(SB.Control Cont in Subcat.Controls)
                    {
                        IConverter<Forms.Control> Converter = ProcureState(Cont);

                        if(Converter != null)
                        {
                            CatPanel.Controls.Add(Converter.Construct(Cont), 1, CatIndex++);
                            State.Add(Converter);
                        }
                    }
                }

                Tabbed.Controls.Add(Page);
            }

            Tabbed.AutoSize = true;
            var S = Tabbed.Size;
            S.Height += 50;
            Tabbed.Size = S;
            Win.Controls.Add(Tabbed);

            Button OK = new Button();
            OK.Parent = Win;
            OK.Text = "OK";
            OK.Location = new System.Drawing.Point(Win.Width-108, Win.Height-50);
            OK.Size = new System.Drawing.Size(100, 25);

            Button Cancel = new Button();
            Cancel.Text = "Cancel";
            Cancel.Parent = Win;
            Cancel.Anchor = AnchorStyles.Left;
            Cancel.Location = new System.Drawing.Point(Win.Width-216, Win.Height-50);
            Cancel.Size = new System.Drawing.Size(100, 25);

            Cancel.Click += CancelClick;
            OK.Click += OKClick;

            Win.AutoSize = true;
            Win.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        void OKClick (object sender, EventArgs e)
        {
            Save();
            Win.DialogResult = DialogResult.OK;
        }

        void CancelClick (object sender, EventArgs e)
        {
            Win.DialogResult = DialogResult.Cancel;
        }

        protected override bool Run ()
        {
            Win.Visible = false;
            return Win.ShowDialog() == DialogResult.OK;
        }

        public override void Show ()
        {
            Win.Show();
        }

        public override void Hide ()
        {
            Win.Hide();
        }
    }
}
