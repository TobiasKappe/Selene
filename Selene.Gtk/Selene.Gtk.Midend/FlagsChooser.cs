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
