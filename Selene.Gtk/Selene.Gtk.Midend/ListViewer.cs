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
        CellRenderer[] Renderers;

        protected override ModalPresenterBase<Widget> MakeDialog()
        {
            return new NotebookDialog<object>(DialogTitle);
        }

        protected override Widget Construct (Type[] Types)
        {
            IdColumn = Types.Length-1;
            Renderers = new CellRenderer[Types.Length-1];

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
            
            View.KeyReleaseEvent += HandleViewKeyReleaseEvent;
            View.KeyPressEvent += HandleViewKeyPressEvent;
            
            return Separator;
        }

        // HACK: We connect to this event solely to disable the error bell when the 
        // delete key is pressed. When it is released, the bell is enabled again.
        // The Glib.ConnectBefore attribute makes sure we're the first to receive it.
        [GLib.ConnectBefore]
        void HandleViewKeyPressEvent (object o, KeyPressEventArgs args)
        {
            if(args.Event.State == Gdk.ModifierType.None && AllowsRemove && args.Event.Key == Gdk.Key.Delete)
                View.Settings.SetLongProperty("gtk-error-bell", 0, null);
        }

        void HandleViewKeyReleaseEvent (object o, KeyReleaseEventArgs args)
        {
            if(args.Event.State == Gdk.ModifierType.None && AllowsRemove && args.Event.Key == Gdk.Key.Delete)
            {
                HandleRemoveClicked(o, args);
                View.Settings.SetLongProperty("gtk-error-bell", 1, null); // Enable the bell again
                
                // We should be the last to process this event.
                args.RetVal = true; 
            }
            else args.RetVal = false;
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
                (Renderer as CellRendererToggle).Activatable = true;
                (Renderer as CellRendererToggle).Toggled += CellToggled;
                View.AppendColumn(Name, Renderer, "active", CurrentColumn++);
            }
            else
            {
                Renderer = new CellRendererText();
                (Renderer as CellRendererText).Editable = true;
                (Renderer as CellRendererText).Edited += CellEdited;
                View.AppendColumn(Name, Renderer, "text", CurrentColumn++);
            }

            Renderers[CurrentColumn-1] = Renderer;
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

        #region Row changing
        TreeIter GetIter(string Path)
        {
            TreeIter Iter;
            Store.GetIter(out Iter, new TreePath(Path));
            return Iter;
        }

        int LookupColumn(object Sender)
        {
            for(int i = 0; i < Renderers.Length; i++)
            {
                if(Sender.Equals(Renderers[i]))
                    return i;
            }

            throw new Exception("Could not look up column");
        }

        void CellToggled(object o, ToggledArgs args)
        {
            int Column = LookupColumn(o);
            TreeIter Iter = GetIter(args.Path);

            bool Value = (Store.GetValue(Iter, Column) as bool?).Value;
            Store.SetValue(Iter, Column, !Value);

            ChangeRow(Iter);
        }

        void CellEdited(object o, EditedArgs args)
        {
            int Column = LookupColumn(o);
            TreeIter Iter = GetIter(args.Path);

            Store.SetValue(Iter, Column, args.NewText);

            ChangeRow(Iter);
        }

        void ChangeRow(TreeIter Changed)
        {
            // IdColumn happens to be the same number as the number of columns
            object[] Items = new object[IdColumn];

            for(int i = 0; i < Renderers.Length; i++)
                Items[i] = Store.GetValue(Changed, i);

            int Id = (Store.GetValue(Changed, IdColumn) as int?).Value;

            RowChanged(Id, Items);
        }
        #endregion
    }
}
