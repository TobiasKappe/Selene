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
		
		public override Widget Content(T Present)
		{
			return Embed(Present, MainBox);
		}
		
		public TreeStoreDialog(string Title) : base(Title)
		{
			Store = new TreeStore(typeof(string), typeof(int));
			View = new TreeView(Store);
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
		
		protected override void Build ()
		{			
			int i = 0;
			foreach(ControlCategory Cat in Manifest.Categories)
			{
				TreeIter Parent = Store.AppendValues(Cat.Name, i);
				foreach(ControlSubcategory Subcat in Cat.Subcategories)
				{
					if(Cat.Subcategories.Length > 1) Store.AppendValues(Parent, Subcat.Name, i);
					CategoryTable SubcatTable = new CategoryTable(Subcat);
					Book.AppendPage(SubcatTable, null);
					i++;
				}
			}
			
			Frame BorderMaker = new Frame();
			BorderMaker.Add(View);
			MainBox.PackStart(BorderMaker);
			MainBox.Add(Book);
			
			View.AppendColumn("Subcategory", new CellRendererText(), "text", 0);
			View.HeadersVisible = false;
			View.CursorChanged += HandleCursorChanged; 
			View.CheckResize();
			MainBox.Spacing = 5;
			Book.ShowTabs = false;
			Book.ShowBorder = false;
		}
	}
}
