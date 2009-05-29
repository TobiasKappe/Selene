using System;
using Selene.Gtk.Midend;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Frontend
{
	public class NotebookDialog<T> : DisplayBase<T>, IEmbeddable<T, Widget> where T : class
	{
		protected Notebook Book;
		protected Dialog Win;
		protected bool mIsEmbedded = false;
		protected HBox MainBox;
		protected bool HasRun = false;
		protected bool PerSubcat = false;
		
		private bool HasButtons = false;
		
		public virtual Widget Content(T Present) 
		{
			return Embed(Present, Book);				
		}
		
		public bool IsEmbedded {
			get { return mIsEmbedded; }
		}
		
		protected Widget Embed(T Present, Widget Ret)
		{
			Prepare(Present);
			mIsEmbedded = true;
			return Ret;
		}
		
		public NotebookDialog(string Title)  : base()
		{
			Book = new Notebook();
			Win = new Dialog(Title, null, DialogFlags.Modal);
			MainBox = new HBox();
			Win.HasSeparator = false;
			Win.Response += HandleResponse; 
			Win.Resizable = false;
			Win.BorderWidth = 3;
			Win.Modal = true;
		}

		void HandleResponse(object o, ResponseArgs args)
		{
			if(args.ResponseId == ResponseType.Ok) Save();
			Win.Hide();
			FireDone();
		}
		
		public override bool Run (T Present)
		{
			base.Run (Present);
			if(!HasButtons) AddButtons();
			
			bool Ret = Win.Run() == (int)ResponseType.Ok;
			HasRun = true;
			return Ret;
		}
		
		public override void Hide ()
		{
			Win.Hide();
		}

		public override void Show ()
		{
			Win.ShowAll();
		}
		
		protected override void Build()
		{
			foreach(ControlCategory Category in Manifest.Categories)
			{
				
				if(!PerSubcat)
				{
					CategoryTable PageTable = new CategoryTable(Category);
					Book.AppendPage(PageTable, new Label(Category.Name));
				}
				else
				{
					foreach(ControlSubcategory Subcat in Category.Subcategories)
					{
						CategoryTable PageTable = new CategoryTable(Subcat);
						Book.AppendPage(PageTable, new Label(Subcat.Name));
					}
				}
				
				EachCategory(Category);
			}
			
			if(Manifest.Categories.Length == 1) 
			{
				Book.ShowBorder = false;
				Book.ShowTabs = false;
			}
			
			if(!HasRun)
			{
				Win.VBox.Add(MainBox);
				MainBox.Add(Book);
			}
		}
		
		protected virtual void EachCategory(ControlCategory Cat)
		{
			return;
		}
		
		protected virtual void AddButtons()
		{
			Win.AddButton(Stock.Cancel, ResponseType.Cancel);
			Win.AddButton(Stock.Ok, ResponseType.Ok);
			HasButtons = true;
		}
	}
}
