using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    public class ListStoreDialog<T> : QModalDialog<T> where T : class
    {
        QStackedWidget Stack;
        QListWidget List;

        public ListStoreDialog (string Title) : base(Title)
        {
            Stack = new QStackedWidget();
            List = new QListWidget();

            InnerLayout.AddWidget(List, 1);
            InnerLayout.AddWidget(Stack, 2);

            List.SetMaximumSize(150, 16777215);

            QWidget.Connect(List, Qt.SIGNAL("currentRowChanged(int)"), new OneArgDelegate<int>(HandleClick));
        }

        void HandleClick(int Row)
        {
            Stack.SetCurrentIndex(Row);
        }

        protected override void Build (ControlManifest Manifest)
        {
            foreach(ControlCategory Cat in Manifest.Categories)
            {
                CategoryLay Lay = new CategoryLay(null);
                QWidget Widg = new QWidget();

                AddCategory(Lay, Cat);

                Widg.SetLayout(Lay);
                Stack.AddWidget(Widg);

                var Item = new QListWidgetItem(Cat.Name, List);
            }

            if(Manifest.Categories.Length == 1) List.Hide();
        }

    }
}
