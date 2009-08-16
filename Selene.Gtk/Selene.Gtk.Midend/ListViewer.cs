using System;
using System.Collections.Generic;
using System.Reflection;
using Selene.Backend;
using Selene.Gtk.Frontend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class ListViewer : ListViewerBase<Widget>
    {
        TreeView View;
        ListStore Store;
        int IdColumn;
        int CurrentColumn = 0;

        protected override ModalPresenterBase<Widget> MakeDialog()
        {
            return new NotebookDialog<object>(Original.GetFlag<string>());
        }

        protected override Widget Construct (Type[] Types)
        {
            IdColumn = Types.Length-1;

            View = new TreeView();
            Store = new ListStore(Types);
            View.Model = Store;
            Frame BorderMaker = new Frame();
            BorderMaker.Add(View);

            HBox Separator = new HBox();
            Separator.Add(BorderMaker);
            VBox Buttons = new VBox();

            AddButton(Buttons, AllowsAdd, Stock.Add, HandleAddClicked);
            AddButton(Buttons, AllowsRemove, Stock.Remove, HandleRemoveClicked);
            AddButton(Buttons, AllowsEdit, Stock.Edit, HandleEditClicked);
            Separator.PackStart(Buttons, false, false, 0);

            return Separator;
        }

        void AddButton(VBox To, bool DependsOn, string Stock, EventHandler Click)
        {
            Button New;
            if(DependsOn || GreyButtons)
            {
                New = new Button(new Image(Stock, IconSize.Button));
                New.Clicked += Click;
                To.PackStart(New, false, false, 0);
                if(!DependsOn && GreyButtons)
                {
                    New.Sensitive = false;
                }
            }
        }
        
        // If a row is removed, we need to decrease the Id on the other rows,
        // to keep in sync with the List<object> in our parent.
        void UpdateIds(int Removed)
        {           
            TreeIter Begin = TreeIter.Zero;
            Store.GetIterFirst(out Begin);
            do
            {
                if(!Store.IterIsValid(Begin)) continue;

                int? Get = Store.GetValue(Begin, IdColumn) as int?;

                if(Get == null) continue;

                if(Get.Value > Removed)
                    Store.SetValue(Begin, IdColumn, Get.Value-1);
            }
            while(Store.IterNext(ref Begin));
        }

        bool CurrentIter(out TreeIter Ret)
        {
            View.Selection.GetSelected(out Ret);
            
            return !Ret.Equals(TreeIter.Zero);
        }

        void HandleEditClicked(object sender, EventArgs e)
        {
            TreeIter Current;

            if(CurrentIter(out Current))
            {
                int Id = (Store.GetValue(Current, IdColumn) as int?).Value;
                EditRow(Id);
            }
        }

        void HandleRemoveClicked(object sender, EventArgs e)
        {
            TreeIter Current;

            if(CurrentIter(out Current))
            {
                int Id = (Store.GetValue(Current, IdColumn) as int?).Value;
                Store.Remove(ref Current);
                DeleteRow(Id);
                if(Store.IterNChildren() > 0)
                    UpdateIds(Id);
            }
        }

        void HandleAddClicked(object sender, EventArgs e)
        {
            AddRow();
        }

        protected override void AddColumn (string Name, System.Type Type)
        {
            CellRenderer Renderer;

            if(Type == typeof(bool))
            {
                Renderer = new CellRendererToggle();
                View.AppendColumn(Name, Renderer, "active", CurrentColumn++);
            }
            else
            {
                Renderer = new CellRendererText();
                View.AppendColumn(Name, Renderer, "text", CurrentColumn++);
            }
        }

        protected override void RowAdded (int Id, object[] Items)
        {
            TreeIter Iter = Store.AppendValues(Items);
            Store.SetValue(Iter, IdColumn, Id);
        }

        protected override void RowEdited (int Id, object[] Items)
        {
            TreeIter Iter = TreeIter.Zero;

            int i = -1;

            do
            {
                if(Iter.Equals(TreeIter.Zero)) Store.GetIterFirst(out Iter);
                else Store.IterNext(ref Iter);

                i = (Store.GetValue(Iter, IdColumn) as int?).Value;
            }
            while(i != Id);

            for(int j = 0; j < Items.Length; j++)
            {
                Store.SetValue(Iter, j, Items[j]);
            }
        }
        
        protected override bool IsViewable(Type T)
        {
            if(T == typeof(String)) return true;
            if(T == typeof(Int16)) return true;
            if(T == typeof(Int32)) return true;
            if(T == typeof(Int64)) return true;
            if(T == typeof(Boolean)) return true;

            return false;
        }

        protected override void Clear ()
        {
            Store.Clear();
        }
    }
}
