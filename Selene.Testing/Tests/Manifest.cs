using NUnit.Framework;
using Selene.Backend;

using System.IO;
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
        enum Bananas { Good, Yellow, Square }
        const string ManifestFile = "../../manifest.xml";

        [ControlManifest(ManifestFile)]
        class ManifestTest
        {
            public Bananas KudosIf;
            public bool YouGetThis;
        }

        static bool Stub()
        {
            if(!File.Exists(ManifestFile))
            {
                DisplayBase<ManifestTest>.StubManifest(ManifestFile);
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
