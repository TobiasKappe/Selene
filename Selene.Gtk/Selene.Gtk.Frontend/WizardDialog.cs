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
    public class WizardDialog<T> : NonModalPresenterBase<Widget>, IValidatable<T>, IDisposable where T : class, ICloneable
    {
        public Assistant Win;
        IValidator<T> mValidator;

        public IValidator<T> Validator {
            set { mValidator = value; }
            get { return mValidator; }
        }

        T Dummy;
        bool Connected = false;

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

        protected override void Build(ControlManifest Manifest)
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
                if(Dummy == null) Dummy = (Present as T).Clone() as T;
                Save(Dummy);

                bool IsValid = mValidator.CatIsValid(Dummy, Win.CurrentPage);
                Win.SetPageComplete(Win.Children[Win.CurrentPage], IsValid);
            }
        }

        protected override void Run()
        {
            // Prevent unnecessary reflection
            if(mValidator != null && !Connected)
            {
                // Clear any events that might trigger premature saving
                while(Application.EventsPending())
                    Application.RunIteration();

                SubscribeAllChange(HandleChange);
                Connected = true;
            }

            for(int i = 0; i < Win.NPages; i++)
                Win.SetPageComplete(Win.Children[i], mValidator == null);
        }
        
        public override void Show()
        {
            Win.ShowAll();
        }

        public override void Block ()
        {
            while(true)
            {
                if(Done != null && Done.Value) break;
                while(Application.EventsPending())
                    Application.RunIteration();
            }
        }

        public void Dispose()
        {
            Win.Dispose();
        }
    }
}
