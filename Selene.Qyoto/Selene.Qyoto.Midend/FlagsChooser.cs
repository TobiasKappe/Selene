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

using System;
using System.Collections.Generic;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class FlagsChooser : FlagsBase<QObject>
    {
        QButtonGroup Group;
        QListWidget List;
        QConverterProxy<Enum> Proxy;
        int i = 0;

        protected override ControlType DefaultSubtype {
            get { return ControlType.MultiCheck; }
        }
        
        protected override ControlType[] Supported {
            get { return new ControlType[] { ControlType.MultiSelect }; }
        }
        
        protected override IEnumerable<int> SelectedIndices {
            get
            {
                if(Original.SubType == ControlType.MultiCheck)
                {
                    for(int p = 0; p < i; p++)
                        if(Group.Button(p).Checked) yield return p;
                }
                else if(Original.SubType == ControlType.MultiSelect)
                {
                    // No way to get the index of a selected QListWidgetItem, so we just 
                    // walk the list and return the indices of the selected items.
                    for(int p = 0; p < List.Count; p++)
                    {
                        QListWidgetItem Item = List.Item(p);
                        if(Item.IsSelected())
                            yield return p;
                    }
                }
                else throw UnsupportedOverride();
            }
        }

        protected override void ChangeIndex (int Index, bool Selected)
        {
            if(Original.SubType == ControlType.MultiCheck)
                Group.Button(Index).Checked = Selected;
            else if(Original.SubType == ControlType.MultiSelect)
            {
                QListWidgetItem Item = List.Item(Index);
                Item.SetSelected(Selected);
            }
            else throw UnsupportedOverride();
        }

        protected string ResolveType(ControlType Type)
        {
            if(Original.SubType == ControlType.MultiCheck)
                return "buttonPressed(int)";
            else if(Original.SubType == ControlType.MultiSelect)
                return "itemSelectionChanged()";
            else throw UnsupportedOverride();
        }

        protected override QObject Construct ()
        {
            Proxy = new QConverterProxy<Enum>(Original, null);
            Proxy.Resolve = ResolveType;

            if(Original.SubType == ControlType.MultiCheck)
            {
                bool Vertical = false;
                Original.GetFlag<bool>(ref Vertical);
    
                QBoxLayout Lay;
                if(Vertical) Lay = new QVBoxLayout();
                else Lay = new QHBoxLayout();
                Group = new QButtonGroup(Lay);
                Proxy.Widg = Group;
    
                Group.Exclusive = false;

                return Lay;
            }
            else if(Original.SubType == ControlType.MultiSelect)
            {
                List = new QListWidget();
                List.selectionBehavior = QAbstractItemView.SelectionBehavior.SelectRows;
                List.selectionMode = QAbstractItemView.SelectionMode.MultiSelection;
                
                //Proxy.Widg = List;
                
                return List;
            }
            else throw UnsupportedOverride();
        }

        protected override void AddOption (string Value)
        {
            if(Original.SubType == ControlType.MultiCheck)
            {
                QCheckBox Add = new QCheckBox(Value);
                Group.AddButton(Add, i++);
                (Widget as QBoxLayout).AddWidget(Add);
            }
            else if(Original.SubType == ControlType.MultiSelect)
            {
                List.AddItem(Value);
            }
            else throw UnsupportedOverride();
        }

        public override event EventHandler Changed {
            add { Proxy.Changed += value; }
            remove { Proxy.Changed -= value; }
        }
    }
}
