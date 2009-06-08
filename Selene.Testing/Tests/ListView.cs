#if GTK
using NUnit.Framework;
using Selene.Backend;

using System;
using Gtk;

using Selene.Gtk.Frontend;

namespace Selene.Testing
{
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
            Assert.AreEqual(Test.People.Length, 2);

            var Disp2 = new NotebookDialog<EditListGrey>("Add/Remove grey");
            var Test2 = new EditListGrey();
            Assert.IsTrue(Disp2.Run(Test2));

            var Disp3 = new NotebookDialog<EditListInv>("Invisible add/remove");
            var Test3 = new EditListInv();
            Assert.IsTrue(Disp3.Run(Test3));
        }
    }
}
#endif
