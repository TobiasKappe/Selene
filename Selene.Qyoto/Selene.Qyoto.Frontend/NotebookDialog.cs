using System;
using Selene.Backend;
using Selene.Qyoto.Midend;
using System.Collections.Generic;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    public class NotebookDialog<T> : ModalPresenterBase<QObject, T> where T : class
    {
        QDialog Dialog;
        QTabWidget Tabs;
        QPushButton OkButton, CancelButton;
        QVBoxLayout Layout;
        QHBoxLayout Buttons;
        QWidget Page;

        public NotebookDialog(string Title)
        {
            Dialog = new QDialog();
            Tabs = new QTabWidget();
            OkButton = new QPushButton("OK");
            CancelButton = new QPushButton("Cancel");
            Page = new QWidget();
            Page.Hide();
            
            Layout = new QVBoxLayout(Dialog);
            Layout.AddWidget(Page);
            Layout.AddWidget(Tabs);
            Buttons = new QHBoxLayout();
            Buttons.AddWidget(CancelButton);
            Buttons.AddWidget(OkButton);
            Layout.AddLayout(Buttons);
            Layout.sizeConstraint = QLayout.SizeConstraint.SetFixedSize;
            
            Dialog.SetWindowTitle(Title);
            
            QWidget.Connect(OkButton, Qt.SIGNAL("clicked()"), ClickOk);
            QWidget.Connect(CancelButton, Qt.SIGNAL("clicked()"), ClickCancel);
        }

        public void ClickCancel()
        {
            Dialog.Reject();
        }

        public void ClickOk()
        {
            Dialog.Accept();
        }

        protected override void Build ()
        {
            if(Manifest.Categories.Length == 1)
            {
                CategoryLay Lay = new CategoryLay(Page);
                foreach(ControlSubcategory Subcategory in Manifest.Categories[0].Subcategories)
                {
                    if(Manifest.Categories[0].Subcategories.Length > 1) Lay.AddHeading(Subcategory);

                    foreach(Control Cont in Subcategory.Controls)
                    {
                        IConverter<QObject> Converter = ProcureState(Cont);

                        if(Converter != null)
                        {
                            Lay.AddWidget(Cont, Converter.Construct(Cont));
                            State.Add(Converter);
                        }
                    }
                }
                Tabs.Hide();
                Page.Show();
            }
            else
            {
                foreach(ControlCategory Category in Manifest.Categories)
                {
                    Page = new QWidget();
                    CategoryLay Lay = new CategoryLay(Page);
                    foreach(ControlSubcategory Subcategory in Category.Subcategories)
                    {
                        if(Category.Subcategories.Length > 1) Lay.AddHeading(Subcategory);

                        foreach(Control Cont in Subcategory.Controls)
                        {
                            IConverter<QObject> Converter = ProcureState(Cont);

                            if(Converter != null)
                            {
                                Lay.AddWidget(Cont, Converter.Construct(Cont));
                                State.Add(Converter);
                            }
                        }
                    }
                    Lay.AddSpacerItem(new QSpacerItem(0, 0, QSizePolicy.Policy.Expanding, QSizePolicy.Policy.Expanding));
                    Tabs.AddTab(Page, Category.Name);
                }
                Tabs.Show();
            }
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
    }
}
