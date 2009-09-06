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

            OkButton.SetFocus();
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
