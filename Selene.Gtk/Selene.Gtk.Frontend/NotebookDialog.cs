using System;
using System.Collections.Generic;
using Selene.Gtk.Midend;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Frontend
{
    public class NotebookDialog<T> : ModalPresenterBase<T>, IEmbeddable<T, Widget> where T : class
    {
        protected Notebook Book;
        protected Dialog Win;
        protected bool mIsEmbedded = false;
        protected HBox MainBox;
        protected bool HasRun = false;
        protected bool PerSubcat = false;
        
        private bool HasButtons = false;
        
        public virtual Widget Content(T Present) 
        {
            return Embed(Present, Book);                
        }
        
        public bool IsEmbedded {
            get { return mIsEmbedded; }
        }
        
        protected Widget Embed(T Present, Widget Ret)
        {
            mIsEmbedded = true;
            Prepare(Present);
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
            Win.Hide();
        }

        public override bool Run (T Present)
        {
            base.Run (Present);
            if(!HasButtons) AddButtons();
            
            bool Ret = Win.Run() == (int)ResponseType.Ok;
            HasRun = true;
            return Ret;
        }
        
        public override void Hide ()
        {
            Win.Hide();
        }

        public override void Show ()
        {
            Win.ShowAll();
        }
        
        protected override void Build()
        {
            List<Control> StateList = new List<Control>();
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
                            WidgetPair Add = ProcureState(Cont) as WidgetPair;

                            if(Add != null)
                            {
                                PageTable.AddWidget(Add);
                                StateList.Add(Add);
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
                            WidgetPair Add = ProcureState(Cont) as WidgetPair;

                            if(Add != null)
                            {
                                PageTable.AddWidget(Add);
                                StateList.Add(Add);
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

            State = StateList.ToArray();
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
    }
}
