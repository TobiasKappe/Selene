using System;
using System.Collections.Generic;
using Selene.Backend;
using Selene.Qyoto.Frontend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class ListViewer : ListViewerBase<QObject>
    {
        static readonly int TypeString = 10;
        static readonly int TypeBool = 11;

        QTableWidget Table;
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

            Table = new QTableWidget(0, Types.Length-1);
            Table.VerticalHeader().Hide();

            Table.HorizontalHeader().SetResizeMode(QHeaderView.ResizeMode.Stretch);
            Table.SetShowGrid(false);
            Table.selectionBehavior = QTableWidget.SelectionBehavior.SelectRows;
            Table.selectionMode = QTableWidget.SelectionMode.SingleSelection;
            QObject.Connect(Table, Qt.SIGNAL("itemChanged(QTableWidgetItem*)"), new OneArgDelegate<QTableWidgetItem>(HandleChange));

            AddButton(Buttons, AllowsAdd, "Add", AddRow);
            AddButton(Buttons, AllowsRemove, "Remove", HandleRemove);
            AddButton(Buttons, AllowsEdit, "Edit", HandleEdit);
            Buttons.AddStretch(1);

            MainLay.AddWidget(Table);
            MainLay.AddLayout(Buttons);

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
            return new NotebookDialog<QObject>(Original.GetFlag<string>());
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
                     Item.SetText(Items[i].ToString());
                }

                Table.SetItem(Id, i, Item);
                Item.SetTextAlignment((int)Qt.AlignmentFlag.AlignCenter);
            }
        }

        protected override void RowEdited (int Id, object[] Items)
        {
            RowAdded(Id, Items);
        }

        void AddButton(QBoxLayout Lay, bool Depends, string Title, NoArgDelegate Clicked)
        {
            if(Depends)
            {
                QPushButton Add = new QPushButton(Title);
                QObject.Connect(Add, Qt.SIGNAL("clicked()"), Clicked);
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
