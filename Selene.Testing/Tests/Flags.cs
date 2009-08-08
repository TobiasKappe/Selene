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
        class FlagsTest
        {
            #pragma warning disable 0649
            
            public Fruit Choose;
        }

        [Test]
        public void Flags()
        {
            var Test = new NotebookDialog<FlagsTest>("Which is good?");
            FlagsTest Result = new FlagsTest();
            Test.Run(Result);

            Assert.AreEqual(Result.Choose | Fruit.Apple, Result.Choose);
            Assert.AreNotEqual(Result.Choose | Fruit.Orange, Result.Choose);
            Assert.AreEqual(Result.Choose | Fruit.Banana, Result.Choose);
        }
    }
}
