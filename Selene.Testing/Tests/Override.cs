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

        class OverrideTest
        {
            #pragma warning disable 0649

            [Control(Override = ControlType.Radio), ControlFlags(true)]
            public Cat Status;
        }

        [Test]
        public void Override()
        {
            var Test = new NotebookDialog<OverrideTest>("Vertical radiobuttons?");
            OverrideTest Val = new OverrideTest();
            Assert.IsTrue(Test.Run(Val));
        }
    }
}
