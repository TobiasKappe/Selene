using System;
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
	public class NamingPicking
	{
		public class NamingPickingTest
		{
			[Control(Name = "Given name")]
			public string Name;
			
			[Control(Name = "Home directory", Override = ControlType.DirectorySelect)]
			public string HomeDir = "/home/"+System.Environment.UserName;
			[Control(Name = "Favourite file", Override = ControlType.FileSelect)]
			public string Favfile;
			public string Hostname = System.Environment.UserDomainName;
		}
		
		public static void Show()
		{
			IPresenter<NamingPickingTest> Present = 
				new NotebookDialog<NamingPickingTest>("Selene");
			NamingPickingTest Save = new NamingPickingTest();
			Present.Run(Save);
			
		}
	}
}
