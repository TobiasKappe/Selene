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
                Row.InsertWidget(0, LabelWidget, 0, (uint) AlignmentFlag.AlignTop);
                LabelWidget.Indent = HasHeading ? 20 : 0;
            }

            if(Add is QWidget) Row.InsertWidget(1, Add as QWidget, 0, (uint) AlignmentFlag.AlignTop);
            if(Add is QLayout) Row.InsertLayout(1, Add as QLayout, 0);

            InsertLayout(i++, Row, 0);
        }
    }
}
