using System;
using Selene.Backend;
using Selene.Gtk.Midend;
using Gtk;

namespace Selene.Gtk.Frontend
{
	internal class CategoryTable : Table
	{	
		private static AttachOptions Big = AttachOptions.Fill | AttachOptions.Expand;
		
		protected CategoryTable(int Count) : base((uint) Count, 2, false)
		{
			BorderWidth = 5;
			ColumnSpacing = 25;
			RowSpacing = 2;
		}
		
		public CategoryTable(ControlCategory Category) : this(Category.ControlCount+Category.Subcategories.Length)
		{
			uint i = 0;
			
			foreach(ControlSubcategory Subcategory in Category.Subcategories)
			{
				uint Indent = 0;
				if(Category.Subcategories.Length > 1 || Category.Subcategories[0].Name != "Default")
				{
					Label CatLabel = new Label(string.Format("<b>{0}</b>", Subcategory.Name));
					CatLabel.UseMarkup = true;
					CatLabel.SetAlignment(0, 0.5f);
					Attach(CatLabel, 0, 2, i, i+1, Big, AttachOptions.Shrink, 0, 0);
					i++;
					Indent = 20;
				}
				
				foreach(Control Cont in Subcategory.Controls)
				{
					AttachPair(Cont, i, Indent);
					i++;
				}
			}
		}
		
		public CategoryTable(ControlSubcategory Subcat) : this(Subcat.Controls.Length)
		{
			BorderWidth = 10;
			ColumnSpacing = 25;
			RowSpacing = 2;
			
			uint i = 0;
							
			foreach(Control Cont in Subcat.Controls)
			{
				AttachPair(Cont, i, 0);
				i++;
			}	        
		}
		
		private void AttachPair(Control Cont, uint i, uint Padding)
		{
			if(!(Cont is WidgetPair)) return;
			WidgetPair Pair = Cont as WidgetPair;
							
			if(Pair.HasLabel) 
			{
				Pair.Marker = new Label(Pair.Label);
				Pair.Marker.SetAlignment(0, 0.5f);
				Pair.Marker.HeightRequest = 0;
				Attach(Pair.Marker, 0, 1, i, i+1, Big, AttachOptions.Shrink, Padding, 0);
				Attach(Pair.Widget, 1, 2, i, i+1, Big, AttachOptions.Shrink, 0, 0);
			}
			else
			{
				if(i != 0) SetRowSpacing(i-1, 5);
				Attach(Pair.Widget, 0, 2, i, i+1, Big, AttachOptions.Shrink, Padding, 0);
			}
		}
	}
}
