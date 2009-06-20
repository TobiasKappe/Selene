using System;
using Selene.Backend;
using Selene.Gtk.Midend;
using System.Collections.Generic;
using Gtk;

namespace Selene.Gtk.Frontend
{
    public class WizardDialog<T> : NonModalPresenterBase<T>, IValidatable<T> where T : class, ICloneable
    {
        public Assistant Win;
        IValidator<T> mValidator;

        public IValidator<T> Validator {
            set { mValidator = value; }
        }

        T Dummy;

        public WizardDialog(string Title)
        {
            Win = new Assistant();
            Win.Title = Title;
            Win.Modal = true;

            Win.Apply += WinApply;
            Win.Cancel += WinCancel;
        }

        void WinCancel (object sender, EventArgs e)
        {
            Console.WriteLine(sender.GetType());
            Win.Hide();
            Success = false;
        }

        void WinApply (object sender, EventArgs e)
        {
            Save();
            Win.Hide();
            Success = true;
        }

        public override void Hide ()
        {
            Win.Hide();
        }
        
        protected override void Build()
        {
            List<Control> StateList = new List<Control>();
            int i = 0;
            foreach(ControlCategory Cat in Manifest.Categories)
            {
                bool end = i == Manifest.Categories.Length-1;

                CategoryTable Table = new CategoryTable(Cat.ControlCount);
                Table.Homogeneous = false;
                Table.ColumnSpacing = 5;
                foreach(ControlSubcategory Subcat in Cat.Subcategories)
                {
                    if(Cat.Subcategories.Length > 1) Table.AddSubcatHeading(Subcat);
                    foreach(Control Cont in Subcat.Controls)
                    {
                        WidgetPair Add = ProcureState(Cont) as WidgetPair;
                        Add.Converter.ConnectChange(Add, HandleChange);
                        Table.AddWidget(Add);
                        StateList.Add(Add);
                    }
                }

                Win.AppendPage(Table);
                Win.SetPageTitle(Table, Cat.Name);
                Win.SetPageType(Table, end ? AssistantPageType.Confirm : AssistantPageType.Content);
                Win.SetPageComplete(Table, true);
                i++;
            }
            State = StateList.ToArray();
        }

        void HandleChange(object o, EventArgs args)
        {
            if(mValidator != null && Win.CurrentPage >= 0)
            {
                if(Dummy == null) Dummy = Present.Clone() as T;
                Save(Dummy);

                bool IsValid = mValidator.CatIsValid(Dummy, Win.CurrentPage);
                Win.SetPageComplete(Win.Children[Win.CurrentPage], IsValid);
            }
        }

        public override void Run(T Present)
        {
            Prepare(Present);
            Show();
            HandleChange(default(object), default(EventArgs));
        }
        
        public override void Show()
        {
            Win.ShowAll();
        }
    }
}
