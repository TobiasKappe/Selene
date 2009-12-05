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
        ListView Viewer;

        protected override void AddColumn (string Name, Type Type)
        {
            Viewer.Columns.Add(Name, Name, 20);
            Viewer.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        protected override void Clear ()
        {
            // Viewer.Clear() cleans the whole widget
            Viewer.Items.Clear();
        }

        protected override Forms.Control Construct (Type[] Types)
        {
            TableLayoutPanel Panel = new TableLayoutPanel();

            Viewer = new Forms.ListView();
            Viewer.FullRowSelect = true;
            Viewer.AutoSize = true;
            Viewer.View = View.Details;
            Panel.Controls.Add(Viewer, 0, 0);

            return Panel;
        }

        protected override bool IsViewable (Type T)
        {
            // Just strings for the moment
            return T == typeof(string);
        }

        protected override ModalPresenterBase<Forms.Control> MakeDialog ()
        {
            return new NotebookDialog<Forms.Control>(Original.GetFlag<string>());
        }

        protected override void RowAdded (int Id, object[] Items)
        {
            string[] Values = new string[Items.Length];
            for(int i = 0; i < Values.Length; i++)
                Values[i] = Items[i].ToString();

            Viewer.Items.Add(new ListViewItem(Values));

            Viewer.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        protected override void RowEdited (int Id, object[] Items)
        {
            throw new System.NotImplementedException();
        }
    }
}
