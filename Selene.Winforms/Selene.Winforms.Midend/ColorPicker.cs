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
using System.Drawing;
using Selene.Backend;
using Forms = System.Windows.Forms;

namespace Selene.Winforms.Midend
{
    internal static class ColorHelper
    {
        public static readonly ushort Factor = 256;

        public static ushort MakeUshort(byte In)
        {
            return (ushort) (In * Factor);
        }

        public static byte MakeByte(ushort In)
        {
            return (byte) (In / Factor);
        }
    }

    public class ColorPicker : ConverterBase<Forms.Control, ushort[]>
    {
        Forms.PictureBox Box;
        Forms.ColorDialog Dialog;

        Image CurrentImage;
        Color CurrentColor;
        Graphics Gr;
        Pen Border;

        EventHandler OnChanged;

        protected override ushort[] ActualValue {
            get
            {
                return new ushort[] { ColorHelper.MakeUshort(CurrentColor.R),
                    ColorHelper.MakeUshort(CurrentColor.G), ColorHelper.MakeUshort(CurrentColor.B) };
            }
            set
            {
                if(value != null)
                {
                    CurrentColor = Color.FromArgb(ColorHelper.MakeByte(value[0]),
                        ColorHelper.MakeByte(value[1]), ColorHelper.MakeByte(value[2]));
                }
                else CurrentColor = Color.Black;

                UpdatePicture();

                if(OnChanged != null)
                    OnChanged(CurrentColor, null);
            }
        }
		
        protected override ControlType DefaultSubtype {
            get { return ControlType.Color; }
        }

        protected override Forms.Control Construct ()
        {
            CurrentColor = new Color();
            CurrentImage = new Bitmap(25, 25);
            Gr = Graphics.FromImage(CurrentImage);
            Border = new Pen(Color.Black);

            Box = new Forms.PictureBox();
            Box.Image = CurrentImage;

            Dialog = new Forms.ColorDialog();
            Box.Click += BoxClick;
            Box.VisibleChanged += BoxVisibleChanged;

            return Box;
        }

        void BoxVisibleChanged (object sender, EventArgs e)
        {
            UpdatePicture();
        }

        void BoxClick (object sender, EventArgs e)
        {
            if(Dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                CurrentColor = Dialog.Color;
                UpdatePicture();

                if(OnChanged != null)
                    OnChanged(CurrentColor, null);
            }
        }

        void UpdatePicture()
        {
            if(Box.Visible)
            {
                Brush Br = new SolidBrush(CurrentColor);
                Gr.FillRectangle(Br, 0, 0, 25, 25);
                Gr.DrawRectangle(Border, 0, 0, 24, 24);
                Box.Refresh();
            }
        }

        public override event EventHandler Changed {
            add { OnChanged += value; }
            remove { OnChanged -= value; }
        }
    }
}
