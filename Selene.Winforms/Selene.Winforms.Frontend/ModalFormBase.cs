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

using Selene.Backend;
using Selene.Winforms.Ordering;

using SB = Selene.Backend;
using Forms = System.Windows.Forms;

namespace Selene.Winforms.Frontend
{
    public abstract class ModalFormBase<T> : ModalPresenterBase<Forms.Control>, IDisposable, IEmbeddable<Forms.Control, T>
    {
        static ModalFormBase()
        {
            CacheConverters();
        }
        
        protected Form Win;
        protected ControlVBox MainBox;
        protected ControlHBox ButtonBox;
        protected bool mIsEmbedded = false;

        internal Form Owner;

        public bool IsEmbedded {
            get { return mIsEmbedded; }
        }
        
        public override bool Visible {
            get { return Win.Visible; }
        }

        protected abstract Forms.Control ActualWidget {
            get;
        }

        public Forms.Control Content(T Present)
        {
            mIsEmbedded = true;
            Prepare(typeof(T), Present, false);

            return ActualWidget;
        }

        public ModalFormBase(string Title)
        {
            Win = new Form();

            Win.Text = Title;
            Win.MaximizeBox = false;
            Win.FormBorderStyle = FormBorderStyle.FixedDialog;
            Win.AutoSize = true;
            Win.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            MainBox = new ControlVBox(2,5);
            ButtonBox = new ControlHBox();

            Button Cancel = new Button { Text = "Cancel" };
            ButtonBox.Controls.Add(Cancel);

            Button OK = new Button { Text = "OK" };
            ButtonBox.Controls.Add(OK);

            Cancel.Click += CancelClick;
            OK.Click += OKClick;

            Win.Controls.Add(MainBox);
        }

        protected override void Build (Selene.Backend.ControlManifest Manifest)
        {
            MainBox.Controls.Add(ButtonBox);
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

            if(Owner != null)
                return Win.ShowDialog(Owner) == DialogResult.OK;
            else return Win.ShowDialog() == DialogResult.OK;
        }

        public override void Show ()
        {
            Win.Show();
        }

        public override void Hide ()
        {
            Win.Hide();
        }

        public void Dispose ()
        {
            Win.Dispose();
        }
    }
}
