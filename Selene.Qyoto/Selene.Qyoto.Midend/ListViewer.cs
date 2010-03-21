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
using Selene.Qyoto.Frontend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    // Proxy class to gain access to the KeyReleaseEvent 
    // needed for binding to the delete key
    class QTableWidgetProxy : QTableWidget
    {
        public SlotFunc DeleteKey;
        
        public QTableWidgetProxy (int rows, int columns) : base(rows, columns)
        {
        }
        
        protected override void KeyReleaseEvent (QKeyEvent ev)
        {
            if(ev.Matches(QKeySequence.StandardKey.Delete) && DeleteKey != null)
                DeleteKey();
            else base.KeyPressEvent (ev);
        }
    }
    
    public class ListViewer : ListViewerBase<QObject>
    {
        static readonly int TypeString = 10;
        static readonly int TypeBool = 11;

        QTableWidgetProxy Table;
        int Column = 0;
        Type[] Types;
        int Changes = 0;

        protected override void AddColumn (string Name, Type Type)
        {
            Table.SetHorizontalHeaderItem(Column++, new QTableWidgetItem(Name));
        }

        protected override void Clear ()
        {
            // Clear() wipes all headers, too
            Table.ClearContents();
        }

        protected override QObject Construct (Type[] Types)
        {
            this.Types = Types;
            QHBoxLayout MainLay = new QHBoxLayout();
            QVBoxLayout Buttons = new QVBoxLayout();

            Table = new QTableWidgetProxy(0, Types.Length-1);
            Table.VerticalHeader().Hide();

            Table.HorizontalHeader().SetResizeMode(QHeaderView.ResizeMode.Stretch);
            Table.SetShowGrid(false);
            Table.selectionBehavior = QTableWidget.SelectionBehavior.SelectRows;
            Table.selectionMode = QTableWidget.SelectionMode.SingleSelection;
            QObject.Connect<QTableWidgetItem>(Table, Qt.SIGNAL("itemChanged(QTableWidgetItem*)"), HandleChange);

            AddButton(Buttons, AllowsAdd, "Add", AddRow);
            AddButton(Buttons, AllowsRemove, "Remove", HandleRemove);
            AddButton(Buttons, AllowsEdit, "Edit", HandleEdit);
            Buttons.AddStretch(1);

            MainLay.AddWidget(Table);
            MainLay.AddLayout(Buttons);
            
            Table.HorizontalHeader().HighlightSections = false;
            
            if(AllowsRemove)
                Table.DeleteKey += HandleRemove;
            
            return MainLay;
        }

        void HandleRemove()
        {
            // This function gives all items (ie cells) but we need the rows
            var Items = Table.SelectedItems();
            for(int i = 0; i < Items.Count; i += Types.Length)
            {
                int Row = Items[i].Row();

                // Bonus is that row numbers change, automagically staying in sync with backend
                DeleteRow(Row);
                Table.RemoveRow(Row);
            }
        }

        void HandleEdit()
        {
            var Items = Table.SelectedItems();
            if(Items.Count > 0)
                EditRow(Items[0].Row());
        }

        void HandleChange(QTableWidgetItem Item)
        {
            Changes++;
            if(Changes > (Types.Length-1) * 2) // 2 preliminary changes per column
            {
                int Row = Item.Row();
                object[] Values = RowValues(Row);

                if(Values != null)
                    RowChanged(Row, Values);
            }
        }

        protected override bool IsViewable (Type T)
        {
            return T == typeof(string) || T == typeof(bool);
        }

        protected override ModalPresenterBase<QObject> MakeDialog ()
        {
            return new NotebookDialog<QObject>(DialogTitle);
        }

        protected override void RowAdded (int Id, object[] Items)
        {
            if(Id >= Table.RowCount)
            {
                Table.InsertRow(Id);
            }

            for(int i = 0; i < Items.Length; i++)
            {
                QTableWidgetItem Item;

                if(Types[i] == typeof(bool))
                {
                    Item = new QTableWidgetItem(TypeBool);
                    Item.SetFlags((uint) QListWidget.ItemFlag.ItemIsUserCheckable | (uint) QListWidget.ItemFlag.ItemIsEnabled | Item.Flags());

                    if((Items[i] as bool?).Value)
                        Item.SetCheckState(Qt.CheckState.Checked);
                    else Item.SetCheckState(Qt.CheckState.Unchecked);
                }
                else
                {
                     Item = new QTableWidgetItem(TypeString);
                     Item.SetText(Items[i] == null ? "" : Items[i].ToString());
                }

                Table.SetItem(Id, i, Item);
                Item.SetTextAlignment((int)Qt.AlignmentFlag.AlignCenter);
            }
            Table.SelectionModel().Clear();
        }

        protected override void RowEdited (int Id, object[] Items)
        {
            RowAdded(Id, Items);
        }

        void AddButton(QBoxLayout Lay, bool Depends, string Title, SlotFunc Slot)
        {
            if(Depends || GreyButtons)
            {
                QPushButton Add = new QPushButton(Title);
                if(GreyButtons && !Depends) Add.SetEnabled(false);
                QObject.Connect(Add, Qt.SIGNAL("clicked()"), Slot);
                Lay.AddWidget(Add);
            }
        }

        object[] RowValues(int Row)
        {
            List<object> Ret = new List<object>();
            for(int i = 0; i < Types.Length; i++)
            {
                if(Types[i] == typeof(int)) continue;

                var Item = Table.Item(Row, i);
                if(Item == null) return null;

                if(Item.type() == TypeBool)
                    Ret.Add(Item.CheckState() == Qt.CheckState.Checked);
                else Ret.Add(Item.Text());
            }

            return Ret.ToArray();
        }
    }
}
