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

using Selene.Backend;
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
