using System;
using Selene.Backend;
using Selene.Qyoto.Midend;
using System.Collections.Generic;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    public class WizardDialog<T> : DisplayBase<T> where T : class, ICloneable
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
            }

            public override bool IsComplete ()
            {
                return mComplete;
            }
        }

        QWizard Wiz;
        T Dummy;

        public IValidator<T> Validator;

        public WizardDialog(string Title)
        {
            Wiz = new QWizard();
            Wiz.SetWindowTitle(Title);
        }

        protected override void Build()
        {
            List<Control> States = new List<Control>();
            foreach(ControlCategory Cat in Manifest.Categories)
            {
                QWizardPage Page = new QValidatablePage();
                CategoryLay Lay = new CategoryLay(Page);

                foreach(ControlSubcategory Subcat in Cat.Subcategories)
                {
                    if(Cat.Subcategories.Length > 1) Lay.AddHeading(Subcat);
                    foreach(Control Cont in Subcat.Controls)
                    {
                        WidgetPair Add = ProcureState(Cont) as WidgetPair;
                        Add.Converter.ConnectChange(Add, HandleChange);
                        Lay.AddWidget(Add);
                        States.Add(Add);
                    }
                }

                Page.SetLayout(Lay);
                Wiz.AddPage(Page);
            }

            this.State = States.ToArray();
        }

        void HandleChange(object Sender, EventArgs Args)
        {
            if(Validator != null)
            {
                if(Dummy == null) Dummy = Present.Clone() as T;
                Save(Dummy);

                bool Valid = Validator.CatIsValid(Dummy, Wiz.CurrentId);
                Console.WriteLine(Valid);
                (Wiz.CurrentPage() as QValidatablePage).Complete = Valid;
            }
        }

        public override bool Run (T Present)
        {
            base.Run (Present);

            return true;
        }

        public override void Hide ()
        {
            Wiz.Hide();
        }

        public override void Show ()
        {
            Wiz.Show();
        }
    }
}
