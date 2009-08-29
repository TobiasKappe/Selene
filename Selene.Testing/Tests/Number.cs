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
        class NumberTest
        {
            #pragma warning disable 0649

            public int Answer = 42;

            [ControlFlags(0, 10)]
            public int Bound = 7;

            [ControlFlags(0, 1, true)]
            public int Wrapped = 1;
        }

        [Test]
        public void Numbers()
        {
            NotebookDialog<NumberTest> Test = new NotebookDialog<NumberTest>("Correct entries?");
            var Store = new NumberTest();
            Assert.IsTrue(Test.Run(Store));
            Console.WriteLine(Store.Answer);
            Console.WriteLine(Store.Bound);
        }
    }
}
