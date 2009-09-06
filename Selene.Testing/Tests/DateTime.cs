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
        class DateTimeTest
        {
            #pragma warning disable 0649

#if QYOTO
            [ControlFlags("dd.MM.yyyy")]
#endif
#if GTK
            [ControlFlags(false, false, true, true, false)]
#endif
            public DateTime Now = System.DateTime.Now;
        }

        [Test]
        public void DateTime()
        {
#if QYOTO
            const string Title = "Formatted date";
#endif
#if GTK
            const string Title = "Changed calendar";
#endif
            var Dialog = new NotebookDialog<DateTimeTest>(Title);
            Assert.IsTrue(Dialog.Run(new DateTimeTest()));
        }
    }
}
