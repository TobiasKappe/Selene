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
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    public abstract class QModalDialog<T> : ModalPresenterBase<QObject>, IEmbeddable<QObject, T>, IDisposable where T : class
    {
        protected QDialog Dialog;
        protected QHBoxLayout InnerLayout;
        protected bool mIsEmbedded = false;

        QVBoxLayout Layout;
        QPushButton OkButton, CancelButton;
        QHBoxLayout Buttons;
        string Title;

        public override bool Visible {
            get { return Dialog.IsVisible(); }
        }
        
        public QModalDialog(string Title)
        {
            Dialog = new QDialog();
            OkButton = new QPushButton("OK");           // Localization?
            CancelButton = new QPushButton("Cancel");

            Layout = new QVBoxLayout(Dialog);
            Buttons = new QHBoxLayout();
            InnerLayout = new QHBoxLayout();

            this.Title = Title;
        }

        public QObject Content(T Present)
        {
            mIsEmbedded = true;
            Prepare(typeof(T), Present, false);
            return InnerLayout;
        }

        public bool IsEmbedded {
            get { return mIsEmbedded; }
        }

        void ClickCancel()
        {
            Dialog.Reject();
        }

        void ClickOk()
        {
            Dialog.Accept();
        }

        protected override void Build(ControlManifest Manifest)
        {
            Buttons.AddWidget(CancelButton);
            Buttons.AddStretch(1);
            Buttons.AddWidget(OkButton);

            if(InnerLayout.Parent() == null && !mIsEmbedded)
                Layout.AddLayout(InnerLayout);

            Layout.AddLayout(Buttons);
            Layout.sizeConstraint = QLayout.SizeConstraint.SetFixedSize;

            Dialog.SetWindowTitle(Title);

            QWidget.Connect(OkButton, Qt.SIGNAL("clicked()"), ClickOk);
            QWidget.Connect(CancelButton, Qt.SIGNAL("clicked()"), ClickCancel);
        }

        public override void Hide ()
        {
            Dialog.Hide();
        }

        protected override bool Run()
        {
            if(Dialog.Exec() == 1)
            {
                Save();
                return true;
            }
            else return false;
        }

        public override void Show ()
        {
            Dialog.Show();
        }

        internal void AddCategory(CategoryLay Lay, ControlCategory Cat)
        {
            foreach(ControlSubcategory Subcat in Cat.Subcategories)
            {
                if(Cat.Subcategories.Length > 1) Lay.AddHeading(Subcat);

                AddSubcategory(Lay, Subcat, false);
            }

            Lay.AddStretch();
        }

        internal void AddSubcategory(CategoryLay Lay, ControlSubcategory Subcat)
        {
            AddSubcategory(Lay, Subcat, true);
        }

        void AddSubcategory(CategoryLay Lay, ControlSubcategory Subcat, bool Stretch)
        {
            foreach(Control Cont in Subcat.Controls)
            {
                IConverter<QObject> Converter = ProcureState(Cont);

                if(Converter != null)
                {
                    Lay.AddWidget(Cont, Converter.Construct(Cont));
                    State.Add(Converter);
                }
            }

            if(Stretch) Lay.AddStretch();
        }

        public void Dispose()
        {
            Dialog.Dispose();
        }
    }
}
