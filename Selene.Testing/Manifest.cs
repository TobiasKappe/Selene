using System;
using System.IO;
using Selene.Backend;

#if QYOTO
using Qyoto;
using Selene.Qyoto.Frontend;
#endif

#if GTK
using Gtk;
using Selene.Gtk.Frontend;
#endif

namespace Selene.Testing
{
	public class Manifest
	{
		public enum Bananas { Good, Yellow, Square }
		private const string ManifestFile = "../../manifest.xml";
		
		[ControlManifest(Manifest.ManifestFile)]
		public class ManifestTest
		{
			public Bananas KudosIf;
			public bool YouGetThis;
		}
		
		public static bool Stub()
		{
			if(!File.Exists(ManifestFile))
			{
				DisplayBase<ManifestTest>.StubManifest(ManifestFile);
				return true;
			}
			return false;
		}
		
		public static void Show()
		{
			if(Stub()) return;
			
			var Disp = new NotebookDialog<ManifestTest>("Selene");
			var Feed = new ManifestTest();
			Disp.Run(Feed);
		}
	}
}
