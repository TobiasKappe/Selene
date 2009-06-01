using System;
using System.Collections.Generic;
using System.Reflection;
using Selene.Backend;
using Selene.Gtk.Frontend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class ListViewer : ListViewerBase
    {
        private TreeView View;
        private ListStore Store;
        private int IdColumn;
        private int CurrentColumn = 0;
        
        protected override DisplayBase<object> Construct (Type[] Types, ref Control Cont)
        {
            if(Cont.SubType != ControlType.Default)
                throw new OverrideException (typeof(ListViewer), Cont.SubType, ControlType.Default);
            
            IdColumn = Types.Length-1;
            WidgetPair Pair = new WidgetPair(Cont);

            View = new TreeView();
            Store = new ListStore(Types);
            View.Model = Store;
            Frame BorderMaker = new Frame();
            BorderMaker.Add(View);
            
            HBox Separator = new HBox();
            Separator.Add(BorderMaker);
            VBox Buttons = new VBox();
            Button Add = new Button(new Image(Stock.Add, IconSize.Button));
            Button Remove = new Button(new Image(Stock.Remove, IconSize.Button));
            Button Edit = new Button(new Image(Stock.Edit, IconSize.Button));
            Buttons.PackStart(Add, false, false, 0);
            Buttons.PackStart(Remove, false, false, 0);
            Buttons.PackStart(Edit, false, false, 0);
            Separator.PackStart(Buttons, false, false, 0);
            
            Add.Clicked += HandleAddClicked;
            Remove.Clicked += HandleRemoveClicked;
            Edit.Clicked += HandleEditClicked;
            
            Pair.Widget = Separator;
            Cont = Pair;
            
            return new NotebookDialog<object>(Cont.GetFlag<string>());
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
    }
}
