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
using SB = Selene.Backend;

using System.IO;
using System;

#if QYOTO
using Selene.Qyoto.Frontend;
using TK = Qyoto;
#endif

#if GTK
using Selene.Gtk.Frontend;
using TK = Gtk;
#endif

#if WINDOWS
using Selene.Winforms.Frontend;
#endif

namespace Selene.Testing
{
    public partial class Harness
    {

        enum Bananas { Good, Yellow, Square }
        const string ManifestFile = "../../manifest.xml";

        [SB.ControlManifest(ManifestFile)]
        class ManifestTest
        {
            #pragma warning disable 0649
            
            public Bananas KudosIf;
            public bool YouGetThis;
        }

        static bool Stub()
        {
            if(!File.Exists(ManifestFile))
            {
#if GTK
                SB.ModalPresenterBase<TK.Widget>.StubManifest<ManifestTest>(ManifestFile);
#endif
#if QYOTO
                SB.ModalPresenterBase<TK.QObject>.StubManifest<ManifestTest>(ManifestFile);
#endif
#if WINDOWS
                SB.ModalPresenterBase<System.Windows.Forms.Control>.StubManifest<ManifestTest>(ManifestFile);
#endif
                return true;
            }
            return false;
        }

        [Test]
        public void Manifest()
        {
            if(Stub()) return;

            var Disp = new NotebookDialog<ManifestTest>("OK if loaded correctly");
            var Feed = new ManifestTest();
            Assert.IsTrue(Disp.Run(Feed));
        }
    }
}
