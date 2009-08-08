using System;
using Selene.Backend;
using NUnit.Framework;

#if GTK
using Selene.Gtk.Frontend;
#endif

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

namespace Selene.Testing
{
    public partial class Harness
    {
        class RepeatTest
        {
            #pragma warning disable 0649
            
            public bool Hey;
            public string Hoi;
        }

        [Test]
        public void Repeating()
        {
            NotebookDialog<RepeatTest> Repeater = new NotebookDialog<RepeatTest>("First empty");

            var Test1 = new RepeatTest();
            var Test2 = new RepeatTest();

            Repeater.Run(Test1);
            Repeater.Run(Test2);

            Assert.IsFalse(Test1.Hey);
            Assert.IsEmpty(Test1.Hoi);

            Assert.IsTrue(Test2.Hey);
            Assert.IsNotEmpty(Test2.Hoi);
        }
    }
}
