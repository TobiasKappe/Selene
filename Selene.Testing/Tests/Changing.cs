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
        class Change
        {
            public string Enter;
            [Control(Override = ControlType.FileSelect)]
            public string File;

            public bool Check;

            public AttributeTargets Toggle;
            [Control(Override = ControlType.Radio), ControlFlags(true)]
            public Base64FormattingOptions Choose;

            public DateTime Calendar;

            public ushort[] Color;

            public Enclosed[] List = new Enclosed[] {};
        }

        int TimesChanged = 0;

        [Test]
        public void Changing()
        {
            var Dialog = new NotebookDialog<Change>("Selene");
            Dialog.SubscribeAllChange(HandleChange);
            Dialog.Run(new Change());

            // We stuplidly assume every widget is changed once

#if QYOTO
            Assert.GreaterOrEqual(TimesChanged, 5);
#endif
#if GTK
            Assert.GreaterOrEqual(TimesChanged, 8);
#endif
        }

        void HandleChange(object Sender, EventArgs Args)
        {
            TimesChanged++;
        }
    }
}
