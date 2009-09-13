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
using Selene.Gtk.Midend;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Frontend
{
    public class TreeStoreDialog<T> : NotebookDialog<T> where T : class
    {
        private TreeStore Store;
        private TreeView View;
        int Counter = 0;
        
        public override Widget Content(T Present)
        {
            return Embed(Present, MainBox);
        }
        
        public TreeStoreDialog(string Title) : base(Title)
        {
            Store = new TreeStore(typeof(string), typeof(int));
            MainBox = new HBox();
        }

        private void Switch(TreePath Path)
        {
            TreeIter Iter;
            Store.GetIter(out Iter, Path);
            int i = (Store.GetValue(Iter, 1) as int?).Value;
            
            Book.Page = i;      
        }

        void HandleCursorChanged(object sender, EventArgs e)
        {
            TreePath Path;
            TreeViewColumn Column;
                
            View.GetCursor(out Path, out Column);
            Switch(Path);
        }
        
        protected override void Build (ControlManifest Manifest)
        {
            Counter = 0;
            PerSubcat = true;

            View = new TreeView(Store);

            if(!HasRun)
            {
                Frame BorderMaker = new Frame();
                BorderMaker.Add(View);
                MainBox.PackStart(BorderMaker);
            }

            base.Build(Manifest);

            View.AppendColumn("Subcategory", new CellRendererText(), "text", 0);
            View.HeadersVisible = false;
            View.CursorChanged += HandleCursorChanged;
            View.CheckResize();
            MainBox.Spacing = 5;
            Book.ShowTabs = false;
            Book.ShowBorder = false;
        }
        
        protected override void EachCategory (ControlCategory Cat)
        {
            TreeIter Parent = Store.AppendValues(Cat.Name, Counter);
            foreach(ControlSubcategory Subcat in Cat.Subcategories)
            {
                if(Cat.Subcategories.Length > 1) Store.AppendValues(Parent, Subcat.Name, Counter);
                Counter++;
            }
        }
    }
}
