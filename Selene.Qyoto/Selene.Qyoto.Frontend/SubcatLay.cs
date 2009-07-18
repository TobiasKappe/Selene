using System;
using Selene.Backend;
using Selene.Qyoto.Midend;
using System.Collections.Generic;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    internal class CategoryLay : QVBoxLayout
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

        public void AddWidget(Control Orig, QObject Add)
        {
            QHBoxLayout Row = new QHBoxLayout();

            if(Orig.SubType != ControlType.Check && Orig.SubType != ControlType.Toggle)
            {
                QLabel LabelWidget = new QLabel(Orig.Label);
                Row.InsertWidget(0, LabelWidget, 0, (uint) AlignmentFlag.AlignLeft);
                LabelWidget.Indent = HasHeading ? 20 : 0;
            }

            QWidget Widg = Add as QWidget;
            QLayout Lay = Add as QLayout;

            if(Widg != null) Row.InsertWidget(1, Widg, 0, (uint) AlignmentFlag.AlignLeft);
            if(Lay != null) Row.InsertLayout(1, Lay, 0);

            InsertLayout(i++, Row, 0);
        }
    }
}
