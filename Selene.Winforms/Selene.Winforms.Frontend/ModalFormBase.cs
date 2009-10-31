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
    public abstract class ModalFormBase : ModalPresenterBase<Forms.Control>
    {
        protected Form Win;
        protected TableLayoutPanel MainPanel;

        public ModalFormBase(string Title)
        {
            Win = new Form();

            Win.Text = Title;
            Win.MaximizeBox = false;
            Win.FormBorderStyle = FormBorderStyle.FixedDialog;
            Win.AutoSize = true;
            Win.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            MainPanel = new TableLayoutPanel();
            MainPanel.AutoSize = true;
            MainPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // There's got to be some better way...
            TableLayoutPanel ButtonPanel = new TableLayoutPanel();
            ButtonPanel.AutoSize = true;

            Button OK = new Button();
            OK.Text = "OK";
            OK.Anchor = AnchorStyles.Right;
            ButtonPanel.Controls.Add(OK, 2, 1);

            Button Cancel = new Button();
            Cancel.Text = "Cancel";
            ButtonPanel.Controls.Add(Cancel, 1, 1);

            Cancel.Click += CancelClick;
            OK.Click += OKClick;

            MainPanel.Controls.Add(ButtonPanel, 1, 2);

            Win.Controls.Add(MainPanel);
        }

        protected void OKClick (object sender, EventArgs e)
        {
            Save();
            Win.DialogResult = DialogResult.OK;
        }

        protected void CancelClick (object sender, EventArgs e)
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
