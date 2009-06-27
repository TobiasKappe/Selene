using System;
using Selene.Gtk.Midend;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Frontend
{
    public class ListStoreDialog<T> : NotebookDialog<T> where T: class
    {
        private TreeView View;
        private ListStore Store;
        private int i;
        
        public override Widget Content (T Present)
        {
            return Embed(Present, MainBox);
        }
        
        public ListStoreDialog(string Title) : base (Title)
        {
            MainBox = new HBox();
            Store = new ListStore(typeof(string), typeof(int));
        }
        
        protected override void Build ()
        {
            View = new TreeView(Store);
            View.AppendColumn("Category", new CellRendererText(), "text", 0);
            View.HeadersVisible = false;
            Frame BorderMaker = new Frame();
            
            if(!HasRun)
            {
                BorderMaker.Add(View);
                MainBox.Add(BorderMaker);
            }
            
            base.Build();

            Book.ShowTabs = false;
            Book.ShowBorder = false;
            View.WidthRequest = 100;
            View.RowActivated += HandleRowActivated;
            View.CursorChanged += HandleCursorChanged;
            BorderMaker.BorderWidth = 5;
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

        void HandleRowActivated(object o, RowActivatedArgs args)
        {
            Switch(args.Path);
        }
        
        protected override void EachCategory(ControlCategory Cat)
        {
            Store.AppendValues(Cat.Name, i);
            i++;
        }
    }
}
