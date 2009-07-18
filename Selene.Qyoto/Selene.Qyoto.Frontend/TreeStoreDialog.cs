using System;
using System.Collections.Generic;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    public class TreeStoreDialog<T> : QModalDialog<T> where T : class
    {
        QTreeWidget Tree;
        QStackedWidget Stack;

        public TreeStoreDialog (string Title) : base(Title)
        {
            Tree = new QTreeWidget();
            Stack = new QStackedWidget();

            InnerLayout.AddWidget(Tree);
            InnerLayout.AddWidget(Stack);

            Tree.SetMaximumSize(150, 16777215);

            QWidget.Connect(Tree, Qt.SIGNAL("currentItemChanged (QTreeWidgetItem*, QTreeWidgetItem*)"),
                            new TwoArgDelegate<QTreeWidgetItem, QTreeWidgetItem>(HandleClick));
        }

        void HandleClick(QTreeWidgetItem Current, QTreeWidgetItem Prev)
        {
            Stack.SetCurrentIndex(int.Parse(Current.Text(1)));
        }

        protected override void Build (ControlManifest Manifest)
        {
            int i = 0;
            foreach(ControlCategory Cat in Manifest.Categories)
            {
                var Current = new QTreeWidgetItem(Tree, new List<string> { Cat.Name, i.ToString() });

                foreach(ControlSubcategory Subcat in Cat.Subcategories)
                {
                    QWidget Widg = new QWidget();

                    if(Cat.Subcategories.Length > 1)
                        new QTreeWidgetItem(Current, new List<string> { Subcat.Name, i.ToString() });

                    CategoryLay Lay = new CategoryLay(Widg);
                    AddSubcategory(Lay, Subcat);

                    Stack.AddWidget(Widg);

                    i++;
                }
            }

            Tree.Header().Hide();
        }
    }
}
