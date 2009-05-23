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
		
		public NotebookDialog(string Title) 
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
			if(Win.VBox.Children.Length == 1) Win.VBox.Add(MainBox);
			if(MainBox.Children.Length == 0) MainBox.Add(Book);
			base.Run (Present);
			if(!HasButtons) AddButtons();
			
			return Win.Run() == (int)ResponseType.Ok;
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
				CategoryTable PageTable = new CategoryTable(Category);
				Book.AppendPage(PageTable, new Label(Category.Name));
				EachCategory(Category);
			}
			
			if(Manifest.Categories.Length == 1) 
			{
				Book.ShowBorder = false;
				Book.ShowTabs = false;
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
