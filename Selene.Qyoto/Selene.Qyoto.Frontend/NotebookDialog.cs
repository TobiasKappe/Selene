using System;
using Selene.Backend;
using Selene.Qyoto.Midend;
using System.Threading;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    public class NotebookDialog<T> : DisplayBase<T> where T : class
    {
        private QDialog Dialog;
        private QTabWidget Tabs;
        private QPushButton OkButton, CancelButton;
        private QVBoxLayout Layout;
        private QHBoxLayout Buttons;
        private QWidget Page;
        
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
            FireDone();
        }
        
        public void ClickOk()
        {
            Dialog.Accept();
            FireDone();
        }
        
        protected override void Build ()
        {   
            if(Manifest.Categories.Length == 1)
            {
                CategoryLay Lay = new CategoryLay(Page);
                foreach(ControlSubcategory Subcategory in Manifest.Categories[0].Subcategories)
                {
                    Lay.Add(Subcategory, !(Manifest.Categories[0].Subcategories.Length == 1 && 
                            Manifest.Categories[0].Subcategories[0].Name == "Default"));
                }               
                Tabs.Hide();
                Page.Show();
                
                return;
            }
            
            foreach(ControlCategory Category in Manifest.Categories)
            {
                Page = new QWidget();
                CategoryLay Lay = new CategoryLay(Page);
                foreach(ControlSubcategory Subcategory in Category.Subcategories)
                {
                    Lay.Add(Subcategory, Category.Subcategories.Length > 1);
                }
                Tabs.AddTab(Page, Category.Name);
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
