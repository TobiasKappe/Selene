using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class BoolToggler : ConverterBase<Widget, bool>
    {
        protected override bool ActualValue {
            get {
                // We can assume the widget derives from ToggleButton because ConverterBase would
                // have thrown an exception on anything else than Check, Toggle or Default
                return (Widget as ToggleButton).Active;
            }
            set {
                // Checkbutton inherits from ToggleButton, so this covers both
                (Widget as ToggleButton).Active = value;
            }
        }

        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.Check, ControlType.Toggle };
            }
        }

        protected override Widget Construct ()
        {
            if(Original.SubType == ControlType.Check || Original.SubType == ControlType.Default)
            {
                Original.SubType = ControlType.Check;
                return new CheckButton(Original.Label);
            }
            else if(Original.SubType == ControlType.Toggle)
                return new ToggleButton(Original.Label);
            else return null;
        }

        public override event EventHandler Changed {
            add
            {
                (Widget as ToggleButton).Toggled += value;
            }
            remove
            {
                (Widget as ToggleButton).Toggled -= value;
            }
        }

    }
}
