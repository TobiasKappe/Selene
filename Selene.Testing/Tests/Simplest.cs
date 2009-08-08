using NUnit.Framework;
using Selene.Backend;

using System;
using Qyoto;
using Gtk;

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

#if GTK
using Selene.Gtk.Frontend;
#endif

namespace Selene.Testing
{
    public partial class Harness
    {
        enum Sex { Male, Female }

        class Test
        {
            #pragma warning disable 0649
            
            public string Surname;
            public Sex Gender;
            public bool Newsletter;
        }

        [Test]
        public void Simplest()
        {
            var Present = new NotebookDialog<Test>("Doe, Female, Yes");
            var SaveTo = new Test();
            Assert.IsTrue(Present.Run(SaveTo));
            Assert.AreEqual(SaveTo.Surname, "Doe");
            Assert.AreEqual(SaveTo.Gender, Sex.Female);
            Assert.IsTrue(SaveTo.Newsletter);
        }
    }
}
