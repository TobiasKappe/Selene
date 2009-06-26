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
            bool mCheckMe = false;

            public bool CheckMe {
                get { return mCheckMe; }
                set { mCheckMe = value; }
            }

            public bool FieldChecker;
        }


        [Test]
        public void Property()
        {
            var Dialog = new NotebookDialog<PropertyTest>("Selene");
            var Test = new PropertyTest();

            Dialog.Run(Test);

            Console.WriteLine(Test.CheckMe);
            Console.WriteLine(Test.FieldChecker);
        }
    }
}
