using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
	public class ColorPicker : ConverterBase<WidgetPair, ushort[]>
	{
		protected override ushort[] ToValue (WidgetPair Start)
		{
			ColorButton Button = Start.Widget as ColorButton;
			return new ushort[] { Button.Color.Red, Button.Color.Green, Button.Color.Blue };
		}
		
		private Gdk.Color GetColor(ref ushort[] Value)
		{
			if(Value == null) Value = new ushort[] { 0, 0, 0 };
			
			Gdk.Color Ret = new Gdk.Color();
			Ret.Red = Value[0];
			Ret.Green = Value[1];
			Ret.Blue = Value[2];
			
			return Ret;
		}
		
		protected override void SetValue (WidgetPair Original, ushort[] Value)
		{
			Gdk.Color C = GetColor(ref Value);
			
			if(Original.Widget is ColorButton)
				(Original.Widget as ColorButton).Color = C;
		}

		protected override WidgetPair ToWidget (WidgetPair Original, ushort[] Value)
		{
			Gdk.Color C = GetColor(ref Value);
			
			if(Value.Length == 3)
			{
				ColorButton Button = new ColorButton(C);
				Original.Widget = Button;
				Original.Expands = false;
				
				return Original;
			}
			
			return null;
		}
	}
}
