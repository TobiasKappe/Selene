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
        class Container
        {
            [ControlFlags("Person dialog")]
            public Enclosed[] People = new Enclosed[] {
                new Enclosed { Name = "Lisa Cuddy", Single = true } };
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
        }
    }
}
#endif
