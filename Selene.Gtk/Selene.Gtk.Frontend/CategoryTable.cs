using System;
using Selene.Backend;
using Selene.Gtk.Midend;
using Gtk;

namespace Selene.Gtk.Frontend
{
    internal class CategoryTable : Table
    {
        uint CurrentRow = 0;
        bool HasHeading = false;

        public CategoryTable(int Rows) : base((uint)Rows, 2, false)
        {
            this.BorderWidth = 6;
            ColumnSpacing = 25;
            RowSpacing = 2;
        }

        public void AddSubcatHeading(ControlSubcategory Subcat)
        {
            Label Heading = new Label(string.Format("<b>{0}</b>", Subcat.Name));
            Heading.UseMarkup = true;
            Heading.SetAlignment(0, 0.5f);
            Attach(Heading, 0, 2, CurrentRow, CurrentRow+1, AttachOptions.Fill, AttachOptions.Shrink, 0, 0);
            CurrentRow++;
            HasHeading = true;
        }

        public void AddWidget(Control Cont, Widget Add)
        {
            Label Marker = new Label(Cont.Label);
            Marker.SetAlignment(0, 0.5f);
            uint Indent = HasHeading ? 20u : 0u;

            if(Cont.SubType != ControlType.Check && Cont.SubType != ControlType.Toggle)
            {
                Attach(Marker, 0, 1, CurrentRow, CurrentRow+1, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, Indent, 0);
                Attach(Add, 1, 2, CurrentRow, CurrentRow+1, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 0, 0);
            }
            else
            {
                Attach(Add, 0, 2, CurrentRow, CurrentRow+1, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, Indent, 0);
            }
            CurrentRow++;
        }
    }
}
