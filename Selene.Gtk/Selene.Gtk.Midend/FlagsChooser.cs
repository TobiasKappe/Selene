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
        TreeView View;
            
        EventHandler Parked;
        EventHandler Proxy;
        int Added = 0;
        
        protected override ControlType DefaultSubtype {
            get { return ControlType.MultiCheck; }
        }
        
        protected override ControlType[] Supported {
            get { return new ControlType[] { ControlType.MultiSelect }; }
        }

        protected override IEnumerable<int> SelectedIndices {
            get
            {
                int i = 0;
                if(Original.SubType == ControlType.MultiCheck)
                {
                    foreach(CheckButton Button in Options.Children)
                    {
                        if(Button.Active) yield return i;
                        i++;
                    }
                }
                else if(Original.SubType == ControlType.MultiSelect)
                {
                    TreeIter Iter;
                    foreach(TreePath Path in View.Selection.GetSelectedRows())
                    {
                        View.Model.GetIter(out Iter, Path);
                        yield return (View.Model.GetValue(Iter, 1) as int?).Value;
                    }
                }
                else throw UnsupportedOverride();
            }
        }

        void HandleChange(object sender, EventArgs args)
        {
            if(Parked != null) Parked(Options, args);
        }

        protected override Widget Construct ()
        {
            if(Original.SubType == ControlType.MultiCheck)
            {
                Proxy += HandleChange;
                bool Vertical = false;
                
                Original.GetFlag<bool>(ref Vertical);
                if(!Vertical) return Options = new HBox();
                else return Options = new VBox();
            }
            else if(Original.SubType == ControlType.MultiSelect)
            {
                View = new TreeView();
                View.Model = new ListStore(typeof(string), typeof(int));
                View.HeadersVisible = false;
                View.AppendColumn("Option", new CellRendererText(), "text", 0);
                View.Selection.Mode = SelectionMode.Multiple;
                View.Selection.Changed += HandleChange;
                
                Frame BorderMaker = new Frame();
                BorderMaker.Add(View);
                
                return BorderMaker;
            }
            else throw UnsupportedOverride();
        }

        protected override void AddOption (string Value)
        {
            if(Original.SubType == ControlType.MultiCheck)
            {
                CheckButton Add = new CheckButton(Value);
                Add.Toggled += Proxy;
                Options.Add(Add);
            }
            else if(Original.SubType == ControlType.MultiSelect)
            {
                (View.Model as ListStore).AppendValues(Value, Added++);
            }
            else throw UnsupportedOverride();
        }

        protected override void ChangeIndex (int Index, bool Selected)
        {
            if(Original.SubType == ControlType.MultiCheck)
            {
                (Options.Children[Index] as CheckButton).Active = Selected;
            }
            else if(Original.SubType == ControlType.MultiSelect)
            {
                TreeIter Iter;
                View.Model.IterNthChild(out Iter, Index);
                
                if(Selected) View.Selection.SelectIter(Iter);
                else View.Selection.UnselectIter(Iter);
            }
            else throw UnsupportedOverride();
        }

        public override event EventHandler Changed {
            add { Parked += value; }
            remove { Parked -= value; }
        }
    }
}
