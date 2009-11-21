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

            QWidget.Connect<QTreeWidgetItem, QTreeWidgetItem>(Tree,
                Qt.SIGNAL("currentItemChanged (QTreeWidgetItem*, QTreeWidgetItem*)"), HandleClick);
        }

        void HandleClick(QTreeWidgetItem Current, QTreeWidgetItem Prev)
        {
            Stack.SetCurrentIndex(int.Parse(Current.Text(1)));
        }

        protected override void Build (ControlManifest Manifest)
        {
            base.Build(Manifest);

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
