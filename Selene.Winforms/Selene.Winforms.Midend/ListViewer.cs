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
using Forms = System.Windows.Forms;
using System.Windows.Forms;
using Selene.Backend;
using Selene.Winforms.Frontend;

namespace Selene.Winforms.Midend
{
    public class ListViewer : ListViewerBase<Forms.Control>
    {
        DataGridView Grid;
        Type[] Types;

        int CurrentSelection {
            get
            {
                return Grid.CurrentRow.Index;
            }
        }

        protected override void AddColumn (string Name, Type Type)
        {
            if(Type == typeof(string))
            {
                DataGridViewTextBoxColumn Col = new DataGridViewTextBoxColumn();
                PrepColumn(Col, Name);
                Col.CellTemplate = new DataGridViewTextBoxCell();
                Grid.Columns.Add(Col);
            }
            if(Type == typeof(bool))
            {
                DataGridViewCheckBoxColumn Col = new DataGridViewCheckBoxColumn();
                PrepColumn(Col, Name);
                Col.CellTemplate = new DataGridViewCheckBoxCell();
                Grid.Columns.Add(Col);
            }
        }
        
        void PrepColumn(DataGridViewColumn Col, string Name)
        {
            Col.Name = Name;
            Col.HeaderText = Name;
            Col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        protected override void Clear ()
        {
            Grid.Rows.Clear();
        }

        protected override Forms.Control Construct (Type[] Types)
        {
            this.Types = Types;
            
            TableLayoutPanel Panel = new TableLayoutPanel();
            TableLayoutPanel ButtonsPanel = new TableLayoutPanel();

            Grid = new DataGridView();
            
            // Mimic a list view
            Grid.AutoGenerateColumns = false;
            Grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            Grid.RowHeadersVisible = false;
            Grid.MultiSelect = false;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToOrderColumns = false;
            
            // Somehow the associated event is not fired
            Grid.AllowUserToDeleteRows = false; 
            
            Grid.CellEndEdit += HandleGridCellEndEdit;
            Panel.Controls.Add(Grid, 0, 0);

            AddButton(ButtonsPanel, AllowsAdd, "Add", 0, AddClicked);
            AddButton(ButtonsPanel, AllowsRemove,  "Remove", 1, RemoveClicked);
            AddButton(ButtonsPanel, AllowsEdit, "Edit", 2, EditClicked);
            ButtonsPanel.AutoSize = true;
            Panel.Controls.Add(ButtonsPanel, 1, 0);
            
            Saving += HandleSaving;

            return Panel;
        }

        void HandleGridCellEndEdit (object sender, DataGridViewCellEventArgs e)
        {
            PatchRow(e.RowIndex, e.ColumnIndex);
        }
        
        void HandleSaving(object Sender, EventArgs e)
        {
            // The user might still have uncommited changes hanging about
            // (e.g. by not leaving the edited column). Make sure we save them.
            // This triggers HandleGridCellEndEdit above.
            Grid.EndEdit();
        }
        
        void PatchRow(int Index, int Column)
        {
            int Cols = Grid.ColumnCount;
            object[] Items = new object[Cols];
            
            for(int i = 0; i < Cols; i++)
                Items[i] = Grid.Rows[Index].Cells[i].Value;
            
            RowChanged(Index, Items);
        }

        protected override bool IsViewable (Type T)
        {
            return T == typeof(string) || T == typeof(bool);
        }

        protected override ModalPresenterBase<Forms.Control> MakeDialog ()
        {
            var Dialog = new NotebookDialog<Forms.Control>(DialogTitle);

            Dialog.Owner = Grid.FindForm();
            return Dialog;
        }

        protected override void RowAdded (int Id, object[] Items)
        {
            Grid.Rows.Add(Items);
        }

        protected override void RowEdited (int Id, object[] Items)
        {
            Grid.Rows[Id].SetValues(Items);
        }

        void AddClicked(object sender, EventArgs args)
        {
            AddRow();
        }

        void RemoveClicked(object sender, EventArgs args)
        {
            int Index = CurrentSelection;
            if(Index < 0) return;

            Grid.Rows.RemoveAt(Index);
            DeleteRow(Index);
        }

        void EditClicked(object sender, EventArgs args)
        {
            int Index = CurrentSelection;
            if(Index < 0) return;

            EditRow(Index);
        }

        void AddButton(TableLayoutPanel Panel, bool DependsOn, string Text, int Row, EventHandler Clicked)
        {
            if(DependsOn || GreyButtons)
            {
                Button Add = new Button();
                Add.Text = Text;
                Add.Enabled = DependsOn;
                Add.Click += Clicked;
                Panel.Controls.Add(Add, 0, Row);
            }
        }
    }
}
