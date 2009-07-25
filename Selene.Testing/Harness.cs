using System;
using NUnit.Framework;
using Gtk;
using Qyoto;

namespace Selene.Testing
{
    [TestFixture]
    public partial class Harness
    {
        [TestFixtureSetUp]
        public void Setup()
        {
#if GTK
            string[] Dummy = new string[] { };
            Init.Check(ref Dummy);
#endif
#if QYOTO
            new QApplication(new string[] { } );
#endif
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            QApplication.Exec();
        }
    }
}
