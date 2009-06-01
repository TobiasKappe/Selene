using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class BoolToggler : ConverterBase<WidgetPair, bool>
    {   
        public BoolToggler() : base(ControlType.Check, ControlType.Toggle)
        {
        }
        
        protected override bool ToValue (WidgetPair Start)
        {
            // We can assume the widget derives from ToggleButton because ConverterBase would
            // have thrown an exception on anything else than Check, Toggle or Default
            return (Start.Widget as ToggleButton).Active;
        }
        
        protected override void SetValue (WidgetPair Original, bool Value)
        {
            // Checkbutton inherits from ToggleButton, so this covers both
            (Original.Widget as ToggleButton).Active = Value;
        }
                
        protected override WidgetPair ToWidget(WidgetPair Original)
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
                
            Original.HasLabel = false;
            Original.Widget = Ret;
                
            return Original;
        }
    }
}
