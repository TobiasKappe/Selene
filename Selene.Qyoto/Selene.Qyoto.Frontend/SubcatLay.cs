// Copyright (c) 2009 Tobias Kappé
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// Except as contained in this notice, the name(s) of the above
// copyright holders shall not be used in advertising or otherwise
// to promote the sale, use or other dealings in this Software
// without prior written authorization.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using Selene.Backend;
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
                if(Orig.Width != 0)
                    Widg.SetFixedWidth(Orig.Width);
                if(Orig.Height != 0)
                    Widg.SetFixedHeight(Orig.Height);
                
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
            else if(Lay != null)
                AddLayout(Lay, i, Col);

            i++;
        }

        public void AddStretch()
        {
            SetRowStretch(i++, 10);
        }
    }
}
