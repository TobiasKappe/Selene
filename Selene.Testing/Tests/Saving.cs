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
        // Used in filling test too
        class MultiTest
        {
            #pragma warning disable 0649

            public string Wuppertahl;
            public int Answer;
            public DateTime Newyear = System.DateTime.Now;
            public bool Check;

            [Control(Override = ControlType.FileSelect)]
            public string Hosts;

            public Direction Back;
            public ushort[] Black = new ushort[] { 0, ushort.MaxValue, 0 };

            [Control(Name = "Apple, orange")]
            public Fruit Fr;
        }

        [Test]
        public void Saving()
        {
            NotebookDialog<MultiTest> Saver = new NotebookDialog<MultiTest>("Do comply");

            var Test = new MultiTest();

            Saver.Run(Test);

            Assert.AreEqual(Test.Wuppertahl.ToLower(), "wuppertahl");
            Assert.AreEqual(Test.Answer, 42);
            Assert.AreEqual(Test.Newyear.DayOfYear, 1);
            Assert.IsTrue(Test.Check);

            Assert.AreEqual(Test.Hosts, "/etc/hosts");
            Assert.AreEqual(Test.Back, Direction.Back);
            Assert.AreEqual(Test.Black, new ushort[] { 0, 0, 0 });
            Assert.AreEqual(Test.Fr, Fruit.Apple | Fruit.Orange);
        }
    }
}
