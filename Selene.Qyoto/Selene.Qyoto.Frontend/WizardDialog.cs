using System;
using Selene.Backend;
using Selene.Qyoto.Midend;
using System.Collections.Generic;
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
            QWidget.Connect(Wiz, Qt.SIGNAL("finished(int)"), new OneArgDelegate<int>(Completed));
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
                if(Done != null && Done.Value) break;
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
