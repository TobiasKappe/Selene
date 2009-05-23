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
	public class Properties
	{
		public enum Cat { Alive, Dead, Neither, Both }
		
		public class PropertiesTest
		{
			[Control(Override = ControlType.Radio), ControlFlags(true)]
			public Cat Status;
		}
		
		public static void Show()
		{
			var Test = new NotebookDialog<PropertiesTest>("Selene");
			PropertiesTest Val = new PropertiesTest();
			Test.Run(Val);
		}
	}
}
