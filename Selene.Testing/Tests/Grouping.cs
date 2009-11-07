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

using NUnit.Framework;
using Selene.Backend;

using System;

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

#if GTK
using Selene.Gtk.Frontend;
#endif

#if WINDOWS
using Selene.Winforms.Frontend;
#endif

namespace Selene.Testing
{
    public partial class Harness
    {
        class GroupingTest : ICloneable
        {
            #pragma warning disable 0649
            
            [Control(Category = "Spare time", Subcategory = "Sports")]
            public string Sport;
            public bool Enjoys;

            [Control(Subcategory = "Preference")]
            public string Music;
            public ushort[] Color;

            [Control(Category = "Employment")]
            public string Employer;
            public DateTime Paycheck;

            public object Clone ()
            {
                return MemberwiseClone();
            }
        }

        const string Title = "OK if categories fit";

        [Test]
        public void Grouping()
        {
            GroupingTest Save = new GroupingTest();

            IModalPresenter Present = new NotebookDialog<GroupingTest>(Title);
            Assert.IsTrue(Present.Run(Save));

            Present = new ListStoreDialog<GroupingTest>(Title);
            Assert.IsTrue(Present.Run(Save));

            Present = new TreeStoreDialog<GroupingTest>(Title);
            Assert.IsTrue(Present.Run(Save));

            var NonModal = new WizardDialog<GroupingTest>(Title);
            NonModal.Run(Save);
            NonModal.Block();
            Assert.IsTrue(NonModal.Success);
        }
    }
}
