// Copyright (c) 2009 Tobias Kappé
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// Except as contained in this notice, the name(s) of the above
// copyright holders shall not be used in advertising or otherwise
// to promote the sale, use or other dealings in this Software
// without prior written authorization.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using NUnit.Framework;
#if GTK
using Gtk;
#endif
#if QYOTO
using Qyoto;
#endif
using System.Reflection;

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

#if QYOTO && NUNIT_HACK
            // HACK: The constructor for QApplication calls System.Reflection.Assembly.GetEntryAssembly(),
            // which returns null when run through nunit (or probably any indirect running method),
            // causing a NullReferenceException. To work around this, we use smoke directly to call
            // the native QApplication constructor. Patched in KDE revision 1055893 (bug #215551)

            var Interceptor = new SmokeInvocation(typeof(QApplication), new QObject());
            string[] args = new string[] { "test" };

            Interceptor.Invoke( "QApplication$?", "QApplication(int&, char**)",
                                typeof(void), typeof(int), args.Length, typeof(string[]), args );
#elif QYOTO
            new QApplication(new string[] { });
#endif

#if WINDOWS
            System.Windows.Forms.Application.EnableVisualStyles();
#endif
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
#if QYOTO
            QApplication.Quit();
#endif
#if WINDOWS
            System.Windows.Forms.Application.Exit();
#endif
        }
    }
}
