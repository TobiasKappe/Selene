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
    public class WizardDialog<T> : NonModalPresenterBase<QObject>, IValidatable<T>, IDisposable where T : class, ICloneable
    {
        // This is actually surprisingly elegant
        class QValidatablePage : QWizardPage
        {
            public bool mComplete = false;

            public bool Complete
            {
                set {
                    mComplete = value;
                    base.Emit.CompleteChanged();
                }
                get { return IsComplete(); }
            }

            public override bool IsComplete ()
            {
                return mComplete;
            }
        }

        QWizard Wiz;
        T Dummy;
        IValidator<T> mValidator;
        int PageCount = 0;

        public IValidator<T> Validator {
            set { mValidator = value; }
            get { return mValidator; }
        }

        public WizardDialog(string Title)
        {
            Wiz = new QWizard();
            Wiz.SetWindowTitle(Title);
            QWidget.Connect<int>(Wiz, Qt.SIGNAL("finished(int)"), Completed);
            QWidget.Connect(Wiz, Qt.SIGNAL("rejected()"), Rejected);
        }

        protected override void Build(ControlManifest Manifest)
        {
            foreach(ControlCategory Cat in Manifest.Categories)
            {
                QWizardPage Page = new QValidatablePage();
                CategoryLay Lay = new CategoryLay(Page);

                foreach(ControlSubcategory Subcat in Cat.Subcategories)
                {
                    if(Cat.Subcategories.Length > 1) Lay.AddHeading(Subcat);
                    foreach(Control Cont in Subcat.Controls)
                    {
                        IConverter<QObject> Converter = ProcureState(Cont);
                        QObject Widget = Converter.Construct(Cont);
                        Converter.Changed += HandleChange;
                        Lay.AddWidget(Cont, Widget);

                        State.Add(Converter);
                    }
                }

                PageCount++;
                Page.SetLayout(Lay);
                Wiz.AddPage(Page);
            }
        }

        void Completed(int Arg)
        {
            Success = Arg == 1;
        }

        void Rejected()
        {
            Console.WriteLine("Rejected");
            Success = false;
        }

        void HandleChange(object Sender, EventArgs Args)
        {
            if(mValidator != null)
            {
                if(Dummy == null) Dummy = (Present as T).Clone() as T;
                Save(Dummy);

                bool Valid = mValidator.CatIsValid(Dummy, Wiz.CurrentId);
                (Wiz.CurrentPage() as QValidatablePage).Complete = Valid;
            }
        }

        public override void Hide ()
        {
            Wiz.Hide();
        }

        public override void Show ()
        {
            Wiz.Show();
        }

        protected override void Run ()
        {
            if(mValidator == null)
            {
                for(int i = 0; i < PageCount; i++)
                    (Wiz.Page(i) as QValidatablePage).Complete = true;
            }

            Show();
        }

        public override void Block ()
        {
            while(true)
            {
                if(Done != null) break;

                while(QApplication.HasPendingEvents())
                    QApplication.ProcessEvents();
            }
        }

        public void Dispose()
        {
            Wiz.Dispose();
        }
    }
}
