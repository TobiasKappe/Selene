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

using System;
using Qyoto;
using Selene.Backend;

namespace Selene.Qyoto.Midend
{
    internal static class ColorHelper
    {
        public static readonly ushort Factor = 256;

        public static ushort MakeUshort(int In)
        {
            return (ushort) (In * Factor);
        }

        public static int MakeInt(ushort In)
        {
            return (int) (In/Factor);
        }

        public static string GetHex(int In)
        {
            byte Ret = (byte) In;
            return Ret.ToString("X").PadLeft(2, '0');
        }

        public static string HtmlName(QColor In)
        {
            return string.Format("#{0}{1}{2}", GetHex(In.Red()), GetHex(In.Green()), GetHex(In.Blue()));
        }

        public static bool AreSame(QColor A, QColor B)
        {
            if(A.Red() != B.Red()) return false;
            if(A.Green() != B.Green()) return false;
            if(A.Blue() != B.Blue()) return false;
            if(A.Alpha() != B.Alpha()) return false;

            return true;
        }
    }

    public class ColorPicker : QConverterProxy<ushort[]>
    {
        QColor Color;

        protected override ushort[] ActualValue {
            get {
                return new ushort[] {
                    ColorHelper.MakeUshort(Color.Red()),
                    ColorHelper.MakeUshort(Color.Green()),
                    ColorHelper.MakeUshort(Color.Green()) };
            }
            set {
                if(value == null)
                    value = new ushort[] { 0, 0, 0 };

                UpdateColor(new QColor(
                    ColorHelper.MakeInt(value[0]),
                    ColorHelper.MakeInt(value[1]),
                    ColorHelper.MakeInt(value[2])));
            }
        }

        void UpdateColor(QColor Update)
        {
            Color = Update;
            QPixmap Map = new QPixmap(16, 16);
            Map.Fill(Color);

            QPainter Paint = new QPainter(Map);
            Paint.SetPen(QColor.FromRgb(0,0,0));
            Paint.DrawRect(0, 0, 15, 15);
            Paint.End();

            (Widget as QPushButton).icon = new QIcon(Map);
            (Widget as QPushButton).Text = ColorHelper.HtmlName(Color);
        }

        protected override Selene.Backend.ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.Default, ControlType.Color } ;
            }
        }

        protected override QObject Construct ()
        {
            QPushButton Ret = new QPushButton();
            QWidget.Connect(Ret, Qt.SIGNAL("clicked()"), Clicked);

            return Ret;
        }

        void Clicked()
        {
            QColor Temp = QColorDialog.GetColor(Color);

            if(Temp != null && Temp.IsValid() && !ColorHelper.AreSame(Temp, Color))
            {
                UpdateColor(Temp);
                FireChanged();
            }
        }
    }
}
