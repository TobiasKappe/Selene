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
