using System;
using Selene.Backend;
using Selene.Qyoto.Midend;
using System.Collections.Generic;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    public class NotebookDialog<T> : QModalDialog<T> where T : class
    {
        QTabWidget Tabs;
        QWidget Page;
        bool HasTabs = false;

        public NotebookDialog(string Title) : base(Title)
        {
            Tabs = new QTabWidget();
            Page = new QWidget();
            Page.Hide();

            InnerLayout.AddWidget(Page);
            InnerLayout.AddWidget(Tabs);
        }

        protected override void Build (ControlManifest Manifest)
        {
            base.Build(Manifest);

            if(Manifest.Categories.Length == 1)
            {
                CategoryLay Lay = new CategoryLay(Page);

                AddCategory(Lay, Manifest.Categories[0]);
                Page.Show();
            }
            else
            {
                HasTabs = true;
                foreach(ControlCategory Category in Manifest.Categories)
                {
                    Page = new QWidget();
                    CategoryLay Lay = new CategoryLay(Page);

                    AddCategory(Lay, Category);

                    Tabs.AddTab(Page, Category.Name);
                }
            }
        }

        protected override bool Run()
        {
            if(HasTabs) Tabs.Show();
            else Tabs.Hide();

            return base.Run();
        }

    }
}
