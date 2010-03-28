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
using Gtk;

namespace Selene.Gtk.Frontend
{
    public class NotebookDialog<T> : ModalPresenterBase<Widget>, IEmbeddable<Widget, T>, IDisposable where T : class
    {
        static NotebookDialog()
        {
            CacheConverters();
        }
        
        protected Notebook Book;
        protected Dialog Win;
        protected bool mIsEmbedded = false;
        protected HBox MainBox;
        protected bool HasRun = false;
        protected bool PerSubcat = false;

        private bool HasButtons = false;
        
        public override bool Visible {
            get { return Win.Visible; }
        }
        
        public virtual Widget Content(T Present)
        {
            return Embed(Present, Book);
        }

        public bool IsEmbedded {
            get { return mIsEmbedded; }
        }

        protected Widget Embed(object Present, Widget Ret)
        {
            mIsEmbedded = true;
            Prepare(typeof(T), Present, false);
            return Ret;
        }

        public NotebookDialog(string Title)  : base()
        {
            Book = new Notebook();
            Win = new Dialog(Title, null, DialogFlags.Modal);
            MainBox = new HBox();
            Win.HasSeparator = false;
            Win.Response += HandleResponse;
            Win.Resizable = false;
            Win.BorderWidth = 3;
            Win.Modal = true;
        }

        void HandleResponse(object o, ResponseArgs args)
        {
            if(args.ResponseId == ResponseType.Ok) Save();
            Hide();
        }

        protected override bool Run ()
        {
            return Win.Run() == (int)ResponseType.Ok;
        }

        public override void Hide ()
        {
            Win.Hide();
        }

        public override void Show ()
        {
            Win.ShowAll();
        }

        protected override void Build(ControlManifest Manifest)
        {
            foreach(ControlCategory Category in Manifest.Categories)
            {
                if(!PerSubcat)
                {
                    CategoryTable PageTable = new CategoryTable(Category.ControlCount + Category.Subcategories.Length);
                    foreach(ControlSubcategory Subcat in Category.Subcategories)
                    {
                        if(Category.Subcategories.Length > 1) PageTable.AddSubcatHeading(Subcat);
                        foreach(Control Cont in Subcat.Controls)
                        {
                            IConverter<Widget> Converter = ProcureState(Cont);

                            if(Converter != null)
                            {
                                PageTable.AddWidget(Cont, Converter.Construct(Cont));
                                State.Add(Converter);
                            }
                        }
                    }
                    Book.AppendPage(PageTable, new Label(Category.Name));
                }
                else
                {
                    foreach(ControlSubcategory Subcat in Category.Subcategories)
                    {
                        CategoryTable PageTable = new CategoryTable(Subcat.Controls.Length);
                        foreach(Control Cont in Subcat.Controls)
                        {
                            IConverter<Widget> Converter = ProcureState(Cont);

                            if(Converter != null)
                            {
                                PageTable.AddWidget(Cont, Converter.Construct(Cont));
                                State.Add(Converter);
                            }
                        }

                        Book.AppendPage(PageTable, new Label(Subcat.Name));
                    }
                }

                EachCategory(Category);
            }

            if(Manifest.Categories.Length == 1)
            {
                Book.ShowBorder = false;
                Book.ShowTabs = false;
            }

            if(!HasRun && !mIsEmbedded)
            {
                Win.VBox.Add(MainBox);
                MainBox.Add(Book);
            }

            if(!HasButtons) AddButtons();
        }

        protected virtual void EachCategory(ControlCategory Cat)
        {
            return;
        }
        
        protected virtual void AddButtons()
        {
            Win.AddButton(Stock.Cancel, ResponseType.Cancel);
            Win.AddButton(Stock.Ok, ResponseType.Ok);
            HasButtons = true;
        }

        public void Dispose()
        {
            Win.Dispose();
        }
    }
}
