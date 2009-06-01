using System;
using System.Reflection;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{   
    public class EnumChooser : EnumBase
    {
        protected override int GetIndex (Control Start)
        {
            WidgetPair Original = Start as WidgetPair;
            if(Original.SubType == ControlType.Radio)
            {
                Box B = (Original.Widget as Box);
                int i;
                for(i = 0; i < B.Children.Length; i++)
                {
                    if((B.Children[i] as RadioButton).Active) return i;
                }
            }
            else if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
                return (Original.Widget as ComboBox).Active;
            
            return 0;
        }

        protected override void SetIndex (Control Start, int Index)
        {
            WidgetPair Original = Start as WidgetPair;
            if(Original.SubType == ControlType.Radio)
            {
                Box B = (Original.Widget as Box);
                (B.Children[Index] as RadioButton).Active = true;
            }
            else if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
            {
                (Original.Widget as ComboBox).Active = Index;
            }
        }

        protected override Control ToWidget (Control Start, string[] Values)
        {
            Widget Ret = null;
            WidgetPair Original = new WidgetPair(Start);

            if(Original.SubType == ControlType.Radio)
            {
                bool Vertical = false;
                Start.GetFlag<bool>(ref Vertical);
                
                Box Box;
                if(Vertical) Box = new VBox();
                else Box = new HBox();
                
                RadioButton FirstBorn = null;
                
                foreach(string Str in Values)
                {
                    if(FirstBorn == null)
                    {
                        FirstBorn = new RadioButton(Str);
                        Box.Add(FirstBorn);
                    }
                    else
                    {
                        RadioButton Button = new RadioButton(FirstBorn, Str);
                        Box.Add(Button);
                    }   
                }
                Ret = Box;
            }
            else if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
            {
                ComboBox Box = ComboBox.NewText();
                foreach(string Str in Values)
                {
                    Box.AppendText(Str);
                }
                Box.Active = 0;
                Ret = Box;
            }
            else throw new OverrideException(typeof(Enum), Original.SubType, ControlType.Dropdown, ControlType.Radio);
            
            Original.Widget = Ret;
            return Original;
        }
    }
}
