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
        [Test]
        public void Filling()
        {
            NotebookDialog<MultiTest> Filler = new NotebookDialog<MultiTest>("All filled?");

            MultiTest Filled =  new MultiTest();
            Filled.Answer = 42;
            Filled.Back = Direction.Back;
            Filled.Black = new ushort [] { 0, 0, 0 };
            Filled.Check = true;
            Filled.Fr = Fruit.Apple | Fruit.Orange;
            Filled.Hosts = "/etc/hosts";
            Filled.Newyear = new DateTime(System.DateTime.Now.Year, 1, 1);
            Filled.Wuppertahl = "wuppertahl";

            Assert.IsTrue(Filler.Run(Filled));
        }
    }
}
