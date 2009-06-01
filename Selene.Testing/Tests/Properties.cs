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
        enum Cat { Alive, Dead, Neither, Both }

        class PropertiesTest
        {
            [Control(Override = ControlType.Radio), ControlFlags(true)]
            public Cat Status;
        }

        [Test]
        public void Properties()
        {
            var Test = new NotebookDialog<PropertiesTest>("Vertical radiobuttons?");
            PropertiesTest Val = new PropertiesTest();
            Assert.IsTrue(Test.Run(Val));
        }
    }
}
