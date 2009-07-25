using System;
using System.Reflection;
using System.Collections.Generic;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class EnumChooser : EnumBase<Widget>
    {
        bool FirstUsed = false;
        RadioButton FirstBorn;

        protected override int CurrentIndex {
            get
            {
                if(Original.SubType == ControlType.Radio)
                {
                    Box B = (Widget as Box);
                    int i;
                    for(i = 0; i < B.Children.Length; i++)
                    {
                        if((B.Children[i] as RadioButton).Active) return i;
                    }
                }
                else if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
                    return (Widget as ComboBox).Active;

                return 0;
            }
            set
            {
                if(Original.SubType == ControlType.Radio)
                {
                    Box B = (Widget as Box);
                    (B.Children[value] as RadioButton).Active = true;
                }
                else if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
                {
                    (Widget as ComboBox).Active = value;
                }
            }
        }

        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.Default, ControlType.Radio, ControlType.Dropdown };
            }
        }

        protected override void AddOption (string Value)
        {
            if(Original.SubType == ControlType.Radio)
            {
                RadioButton Button;

                if(!FirstUsed)
                {
                    Button = FirstBorn;
                    Button.Label = Value;
                    FirstUsed = true;
                }
                else Button = new RadioButton(FirstBorn, Value);

                (Widget as Box).Add(Button);
            }
            else (Widget as ComboBox).AppendText(Value);
        }

        protected override Widget Construct ()
        {
            if(Original.SubType == ControlType.Radio)
            {
                bool Vertical = false;
                Original.GetFlag<bool>(ref Vertical);

                Box Box;
                if(Vertical) Box = new VBox();
                else Box = new HBox();

                FirstBorn = new RadioButton("");

                return Box;
            }
            else if(Original.SubType == ControlType.Dropdown || Original.SubType == ControlType.Default)
            {
                return ComboBox.NewText();
            }
            else throw new OverrideException(typeof(Enum), Original.SubType, ControlType.Dropdown, ControlType.Radio);
        }

        void EventChange(EventHandler Subject, bool Add)
        {
            if(Original.SubType == ControlType.Default || Original.SubType == ControlType.Dropdown)
            {
                if(Add) (Widget as ComboBox).Changed += Subject;
                else (Widget as ComboBox).Changed -= Subject;
            }
            else if(Original.SubType == ControlType.Radio)
            {
                if(Add) FirstBorn.Clicked += Subject;
                else FirstBorn.Clicked -= Subject;
            }
        }

        public override event EventHandler Changed {
            add { EventChange(value, true); }
            remove { EventChange(value, false); }
        }
    }
}
