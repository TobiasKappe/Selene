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
        [Flags]
        enum Fruit { Apple = 1, Orange = 2, Banana = 4 }
        enum Direction { Left, Right, Back }

        class Change
        {
            #pragma warning disable 0649

            // Simple widgets
            public bool Check;
            public string Enter;
            [Control(Override = ControlType.FileSelect)]
            public string File;

            // Enum-based widgets
            public Fruit Bowl = Fruit.Orange | Fruit.Apple;
            [Control(Override = ControlType.Radio)]
            public Direction Choose;
            public Direction Choose2;

            // Compound widgets
            public DateTime Calendar;
            public ushort[] Color = new ushort[] { ushort.MaxValue, 0, 0 };
            public Enclosed[] List = new Enclosed[] {};
        }

        int TimesChanged = 0;

        [Test]
        public void Changing()
        {
            var Dialog = new NotebookDialog<Change>("Selene");
            Dialog.SubscribeAllChange<Change>(HandleChange);
            Dialog.Run(new Change());

            // We stuplidly assume every widget is changed once

#if QYOTO
            Assert.GreaterOrEqual(TimesChanged, 9);
#endif
#if GTK
            Assert.GreaterOrEqual(TimesChanged, 15);
#endif
        }

        void HandleChange(object Sender, EventArgs Args)
        {
            TimesChanged++;
        }
    }
}
