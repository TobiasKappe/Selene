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
        class PropertyTest
        {
            #pragma warning disable 0649

            public bool CheckMe {
                get { return !Asd; }
                set { Asd = !value; }
            }
            internal bool Asd;

            public bool FieldChecker;
        }


        [Test]
        public void Property()
        {
            var Dialog = new NotebookDialog<PropertyTest>("Check both");
            var Test = new PropertyTest();

            Dialog.Run(Test);

            Assert.IsTrue(Test.FieldChecker);
            Assert.IsTrue(Test.CheckMe);
            Assert.IsFalse(Test.Asd);
        }
    }
}
