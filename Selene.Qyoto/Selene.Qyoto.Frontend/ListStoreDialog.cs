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
            base.Build(Manifest);

            foreach(ControlCategory Cat in Manifest.Categories)
            {
                CategoryLay Lay = new CategoryLay(null);
                QWidget Widg = new QWidget();

                AddCategory(Lay, Cat);

                Widg.SetLayout(Lay);
                Stack.AddWidget(Widg);

                new QListWidgetItem(Cat.Name, List);
            }

            if(Manifest.Categories.Length == 1) List.Hide();
        }

    }
}
