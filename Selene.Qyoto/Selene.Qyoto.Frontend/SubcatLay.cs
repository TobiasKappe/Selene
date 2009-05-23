using System;
using Selene.Backend;
using Selene.Qyoto.Midend;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
	public class CategoryLay : QVBoxLayout
	{	
		int i = 0;
		
		public CategoryLay(QWidget Parent) : base(Parent)
		{
		}
		
		public CategoryLay() : base()
		{
		}
		
		public void Add(ControlSubcategory Subcat, bool Label)
		{
			if(Label)
			{
				QLabel SubcatLabel = new QLabel(Subcat.Name);
				QFont NewFont = new QFont();
				NewFont.SetBold(true);
				SubcatLabel.Font = NewFont;
				InsertWidget(i++, SubcatLabel);	
			}
			
			foreach(Control Cont in Subcat.Controls)
			{
				WidgetPair WP = Cont as WidgetPair;
				if(WP == null) return;
				
				QHBoxLayout Row = new QHBoxLayout();
				
				if(WP.HasLabel) 
				{
					WP.LabelWidget = new QLabel(Cont.Label);
					Row.InsertWidget(0, WP.LabelWidget);
					WP.LabelWidget.Indent = Label ? 20 : 0;
				}
				if(WP.Widget is QWidget) Row.InsertWidget(1, WP.Widget as QWidget);
				if(WP.Widget is QLayout) Row.InsertLayout(1, WP.Widget as QLayout);
				InsertLayout(i++, Row);
			}
		}
	}
}
