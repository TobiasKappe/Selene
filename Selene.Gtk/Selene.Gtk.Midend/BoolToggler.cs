using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
	public class BoolToggler : ConverterBase<WidgetPair, bool>
	{			
		protected override bool ToValue (WidgetPair Start)
		{
			return (Start.Widget as CheckButton).Active;
		}
		
		protected override void SetValue (WidgetPair Original, bool Value)
		{
			// Checkbutton inherits from ToggleButton, so this covers both
			if(Original.Widget is ToggleButton)
				(Original.Widget as ToggleButton).Active = Value;
		}
				
		protected override WidgetPair ToWidget(WidgetPair Original, bool Value)
		{
			ToggleButton Ret;
			if(Original.SubType == ControlType.Check || Original.SubType == ControlType.Default) 
				Ret = new CheckButton(Original.Label);
			else if(Original.SubType == ControlType.Toggle)
			{
				Ret = new ToggleButton(Original.Label);
				Original.Expands = false;
			}
			else return null;
				
			Ret.Active = Value;
			Original.HasLabel = false;
			Original.Widget = Ret;
				
			return Original;
		}
	}
}
