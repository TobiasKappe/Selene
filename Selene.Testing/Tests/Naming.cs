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
        class NamingPickingTest
        {
            [Control(Name = "Given name")]
            public string Name;
            
            [Control(Name = "Home directory", Override = ControlType.DirectorySelect)]
            public string HomeDir = "/home/"+System.Environment.UserName;
            [Control(Name = "Favourite file", Override = ControlType.FileSelect)]
            public string Favfile;
            public string Hostname = System.Environment.UserDomainName;
        }

        [Test]
        public void NamingPicking()
        {
            var Present = new NotebookDialog<NamingPickingTest>("OK if values match");
            NamingPickingTest Save = new NamingPickingTest();
            Assert.IsTrue(Present.Run(Save));
        }
    }
}
