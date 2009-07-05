using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    public abstract class QModalDialog<T> : ModalPresenterBase<QObject, T> where T : class
    {
        protected QDialog Dialog;
        protected QHBoxLayout InnerLayout;

        QVBoxLayout Layout;
        QPushButton OkButton, CancelButton;
        QHBoxLayout Buttons;

        public QModalDialog(string Title)
        {
            Dialog = new QDialog();
            OkButton = new QPushButton("OK");           // Localization?
            CancelButton = new QPushButton("Cancel");

            Layout = new QVBoxLayout(Dialog);
            Buttons = new QHBoxLayout();
            InnerLayout = new QHBoxLayout();

            Buttons.AddWidget(CancelButton);
            Buttons.AddStretch(1);
            Buttons.AddWidget(OkButton);
            Layout.AddLayout(InnerLayout);
            Layout.AddLayout(Buttons);
            Layout.sizeConstraint = QLayout.SizeConstraint.SetFixedSize;

            Dialog.SetWindowTitle(Title);

            QWidget.Connect(OkButton, Qt.SIGNAL("clicked()"), ClickOk);
            QWidget.Connect(CancelButton, Qt.SIGNAL("clicked()"), ClickCancel);

            OkButton.SetFocus();
        }

        void ClickCancel()
        {
            Dialog.Reject();
        }

        void ClickOk()
        {
            Dialog.Accept();
        }

        public override void Hide ()
        {
            Dialog.Hide();
        }

        public override bool Run (T Present)
        {
            base.Run (Present);

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

                foreach(Control Cont in Subcat.Controls)
                {
                    IConverter<QObject> Converter = ProcureState(Cont);

                    if(Converter != null)
                    {
                        Lay.AddWidget(Cont, Converter.Construct(Cont));
                        State.Add(Converter);
                    }
                }
            }
        }
    }
}
