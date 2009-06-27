using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class ColorPicker : ConverterBase<Widget, ushort[]>
    {
        protected override ushort[] ActualValue {
            get
            {
                ColorButton Button = Widget as ColorButton;
                return new ushort[] { Button.Color.Red, Button.Color.Green, Button.Color.Blue };
            }
            set
            {
                Gdk.Color C = GetColor(value);
                (Widget as ColorButton).Color = C;
            }
        }

        private Gdk.Color GetColor(ushort[] Value)
        {
            if(Value == null) Value = new ushort[] { 0, 0, 0 };

            Gdk.Color Ret = new Gdk.Color();
            Ret.Red = Value[0];
            Ret.Green = Value[1];
            Ret.Blue = Value[2];

            return Ret;
        }

        protected override Widget Construct ()
        {
            return new ColorButton();
        }

        public override event EventHandler Changed {
            add
            {
                (Widget as ColorButton).ColorSet += value;
            }
            remove
            {
                (Widget as ColorButton).ColorSet -= value;
            }
        }
    }
}
