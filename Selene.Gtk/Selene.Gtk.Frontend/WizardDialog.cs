using System;
using Selene.Backend;
using Selene.Gtk.Midend;
using Gtk;

namespace Selene.Gtk.Frontend
{
	public class WizardDialog<T> : DisplayBase<T> where T : class
	{
		public Assistant Win;
		public IValidator<T> Validator;
		
		public WizardDialog(string Title) 
		{
			Win = new Assistant();
			Win.Title = Title;
			Win.Modal = true;
		}
		
		public override void Hide ()
		{
			Win.Hide();
		}
		
		protected override void Build()
		{
			int i = 0;
			foreach(ControlCategory Cat in Manifest.Categories)
			{
				bool end = i == Manifest.Categories.Length-1;
				CategoryTable Table = new CategoryTable(Cat);
				Table.ShowAll();
				
				Win.AppendPage(Table);
				Win.SetPageTitle(Table, Cat.Name);
				Win.SetPageType(Table, end ? AssistantPageType.Confirm : AssistantPageType.Content);
				Win.SetPageComplete(Table, true);
				i++;
			}
			
			Win.WidgetEvent +=HandleWidgetEvent; 
		}

		void HandleWidgetEvent(object o, WidgetEventArgs args)
		{
			if(Validator != null) 
			{
				Save();
				bool IsValid = Validator.CatIsValid(Present, Win.CurrentPage);
				Win.SetPageComplete(Win.Children[Win.CurrentPage], IsValid);
			}
		}
		
		public override bool Run(T Present)
		{
			base.Run(Present);
			Win.Show();
			return true;
		}
		
		public override void Show()
		{
			Win.Show();
		}
	}
}
