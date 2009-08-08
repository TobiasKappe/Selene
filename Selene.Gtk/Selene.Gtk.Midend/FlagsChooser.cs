using Gtk;
using System;
using System.Collections.Generic;
using Selene.Backend;

namespace Selene.Gtk.Midend
{
    public class FlagsChooser : FlagsBase<Widget>
    {
        Box Options;
        EventHandler Parked;
        EventHandler Proxy;

        protected override IEnumerable<int> SelectedIndices {
            get
            {
                int i = 0;
                foreach(CheckButton Button in Options.Children)
                {
                    if(Button.Active) yield return i;
                    i++;
                }
            }
        }

        void HandleChange(object sender, EventArgs args)
        {
            if(Parked != null) Parked(sender, args);
        }

        protected override Widget Construct ()
        {
            Proxy += HandleChange;
            bool Vertical = false;
            Original.GetFlag<bool>(ref Vertical);

            if(!Vertical) return Options = new HBox();
            else return Options = new VBox();
        }

        protected override void AddOption (string Value)
        {
            CheckButton Add = new CheckButton(Value);
            Add.Toggled += Proxy;
            Options.Add(Add);
        }

        protected override void ChangeIndex (int Index, bool Selected)
        {
            (Options.Children[Index] as CheckButton).Active = Selected;
        }

        public override event EventHandler Changed {
            add { Parked += value; }
            remove { Parked -= value; }
        }
    }
}
