using System;
using Selene.Backend;
using Selene.Qyoto.Midend;
using System.Collections.Generic;
using Qyoto;

namespace Selene.Qyoto.Frontend
{
    internal class CategoryLay : QGridLayout
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
            AddWidget(SubcatLabel, i++, 0);
            HasHeading = true;
        }

        public void AddWidget(Control Orig, QObject Add)
        {
            int Col = 0;

            if(Orig.SubType != ControlType.Check && Orig.SubType != ControlType.Toggle)
            {
                QLabel LabelWidget = new QLabel(Orig.Label);
                LabelWidget.Indent = HasHeading ? 20 : 0;

                AddWidget(LabelWidget, i, 0);
                Col++;
            }

            QWidget Widg = Add as QWidget;
            QLayout Lay = Add as QLayout;

            if(Widg != null)
            {
                if(Col == 0 && HasHeading)
                {
                    QHBoxLayout Shift = new QHBoxLayout();
                    QLabel Pusher = new QLabel(" ");
                    Pusher.Indent = 10;
                    Shift.AddWidget(Pusher);
                    Shift.AddWidget(Widg);

                    AddLayout(Shift, i, 0);
                }
                else
                {
                    AddWidget(Widg, i, Col);
                }
            }
            if(Lay != null)
                AddLayout(Lay, i, Col);

            i++;
        }

        public void AddStretch()
        {
            SetRowStretch(i++, 10);
        }
    }
}
