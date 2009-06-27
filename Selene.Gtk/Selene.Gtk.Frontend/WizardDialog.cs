using System;
using Selene.Backend;
using Selene.Gtk.Midend;
using Gtk;

namespace Selene.Gtk.Frontend
{
    public class WizardDialog<T> : NonModalPresenterBase<Widget, T>, IValidatable<T> where T : class, ICloneable
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
            Win.Resizable = false;
        }

        void WinCancel (object sender, EventArgs e)
        {
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

        int counter = 0;
        protected override void Build()
        {
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
                        IConverter<Widget> Converter = ProcureState(Cont);
                        Table.AddWidget(Cont, Converter.Construct(Cont));
                        Converter.Changed += HandleChange;
                        State.Add(Converter);
                    }
                }

                Win.AppendPage(Table);
                Win.SetPageTitle(Table, Cat.Name);
                Win.SetPageType(Table, end ? AssistantPageType.Confirm : AssistantPageType.Content);
                Win.SetPageComplete(Table, false);
                i++;
            }
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
