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
using SB = Selene.Backend;
using System.Windows.Forms;
using Forms = System.Windows.Forms;

namespace Selene.Winforms.Frontend
{
    public class NotebookDialog<T> : ModalPresenterBase<Forms.Control>
    {
        Form Win;
        TabControl Tabbed;

        public NotebookDialog(string Title)
        {
            Win = new Form();

            Win.Name = Title;
            Win.MaximizeBox = false;
            Win.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        protected override void Build (ControlManifest Manifest)
        {
            Forms.Control Root;

            if(Manifest.Categories.Length == 1) Root = Win;
            else
            {
                Tabbed = new TabControl();
                Tabbed.Parent = Win;
                Tabbed.Dock = DockStyle.Fill;
                Root = Tabbed;
            }

            foreach(ControlCategory Cat in Manifest.Categories)
            {
                Forms.Control CatParent;
                TabPage Page = null;

                if(Manifest.Categories.Length == 1) CatParent = Root;
                else
                {
                    Page = new TabPage(Cat.Name);
                    Page.Parent = Root;
                    Page.Dock = DockStyle.Fill;
                    CatParent = Page;
                }

                foreach(ControlSubcategory Subcat in Cat.Subcategories)
                {
                    Forms.Control Parent;

                    if(Cat.Subcategories.Length == 1) Parent = CatParent;
                    else
                    {
                        GroupBox Box = new GroupBox();
                        Box.Text = Subcat.Name;
                        Box.Parent = CatParent;
                        Box.Anchor = AnchorStyles.Top;
                        Box.Dock = DockStyle.Fill;
                        Parent = Box;
                    }

                    foreach(SB.Control Cont in Subcat.Controls)
                    {
                        IConverter<Forms.Control> Converter = ProcureState(Cont);

                        if(Converter != null)
                        {
                            Forms.Control ControlParent;

                            Forms.Control Add = Converter.Construct(Cont);
                            State.Add(Converter);

                            Console.WriteLine(Cont.SubType);
                            if(Cont.SubType != ControlType.Check && Cont.SubType != ControlType.Toggle)
                            {
                                Label Label = new Label();
                                Label.Parent = Parent;
                                Label.Dock = DockStyle.Top;
                                Label.Text = Cont.Label;

                                Add.Parent = Label;
                                Add.Dock = DockStyle.Right;
                            }
                            else
                            {
                                Add.Parent = Parent;
                                Add.Dock = DockStyle.Top;
                            }
                        }
                    }
                }

                if(Manifest.Categories.Length > 1)
                    Tabbed.Controls.Add(Page);
            }
        }

        protected override bool Run ()
        {
            Win.Visible = false;
            return Win.ShowDialog() == DialogResult.OK;
        }

        public override void Show ()
        {
            Win.Show();
        }

        public override void Hide ()
        {
            Win.Hide();
        }
    }
}
