using System;
using Selene.Backend;
using Selene.Qyoto.Midend;
using System.Collections.Generic;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    public class CategoryLay : QVBoxLayout
    {
        int i = 0;
        QFont NewFont;
        bool HasHeading = false;

        public CategoryLay(QWidget Parent) : base(Parent)
        {
            NewFont = new QFont();
            NewFont.SetBold(true);
        }

        public void AddHeading(ControlSubcategory Subcat)
        {
            QLabel SubcatLabel = new QLabel(Subcat.Name);
            SubcatLabel.Font = NewFont;
            InsertWidget(i++, SubcatLabel);
            HasHeading = true;
        }

        public void AddWidget(WidgetPair Add)
        {
            if(Add == null) return;

            QHBoxLayout Row = new QHBoxLayout();

            if(Add.HasLabel)
            {
                Add.LabelWidget = new QLabel(Add.Label);
                Row.InsertWidget(0, Add.LabelWidget);
                Add.LabelWidget.Indent = HasHeading ? 20 : 0;
            }

            if(Add.Widget is QWidget) Row.InsertWidget(1, Add.Widget as QWidget);
            if(Add.Widget is QLayout) Row.InsertLayout(1, Add.Widget as QLayout);

            InsertLayout(i++, Row, 0);
        }
    }
}
