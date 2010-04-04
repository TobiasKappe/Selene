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
    /* Listview test - this unit test checks the implementation of 
     * ListViewerBase provided by the frontend. This test consists of
     * three sub-tests.
     * 
     * Firstly, the user is presented with a dialog. The user should
     * add one "person" to the list. The code then checks whether the
     * length of the list is exactly 2.
     * 
     * Secondly, the test disables the add and remove buttons. The user
     * should see the buttons, but they should be disabled. Press OK to
     * confirm this
     * 
     * Lastly, the second test is re-run but this time not disabling the
     * add and remove buttons, but completely hiding them. Again, press
     * OK to confirm this.
     */
    
    public partial class Harness
    {
        static Enclosed Individual = new Enclosed { Name = "Lisa Cuddy", Single = true };

        class Container
        {
            [ControlFlags("Person dialog")]
            public Enclosed[] People = new Enclosed[] { Individual };
        }

        // Testing these for all permutations is silly, these two suffice
        // The adding of buttons is handled by the same function anyway
        class EditListGrey
        {
            [ControlFlags("Person dialog", true, false, false, true)]
            public Enclosed[] People = new Enclosed[] { Individual };
        }

        class EditListInv
        {
            [ControlFlags("Person dialog", true, false, false, false)]
            public Enclosed[] People = new Enclosed[] { Individual };
        }
        
        class NonEditList
        {
            [ControlFlags("Person dialog", false, true, true, false)]
            public Enclosed[] People = new Enclosed[] { Individual };
        }

        class Enclosed
        {
            public string Name;
            public bool Single;
        }

        [Test]
        public void ListView()
        {
            var Disp = new NotebookDialog<Container>("Add one person");
            var Test = new Container();
            Assert.IsTrue(Disp.Run(Test));
            Assert.AreEqual(2, Test.People.Length);

            var Disp2 = new NotebookDialog<EditListGrey>("Add/Remove grey");
            var Test2 = new EditListGrey();
            Assert.IsTrue(Disp2.Run(Test2));

            var Disp3 = new NotebookDialog<EditListInv>("Invisible add/remove");
            var Test3 = new EditListInv();
            Assert.IsTrue(Disp3.Run(Test3));
            
            var Disp4 = new NotebookDialog<NonEditList>("Disabled inline edit");
            var Test4 = new NonEditList();
            Assert.IsTrue(Disp4.Run(Test4));
        }
    }
}
