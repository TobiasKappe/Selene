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
	public class ListView
	{	
		public class Container
		{
			public Enclosed[] People = new Enclosed[] { 
				new Enclosed { Name = "Lisa Cuddy", Single = true } };
		}
		
		public class Enclosed
		{
			public string Name;
			public bool Single;
		}
		
		public static void Show()
		{
			var Disp = new NotebookDialog<Container>("Selene");
			var Test = new Container();
			Disp.Run(Test);
			
			foreach(Enclosed Person in Test.People)
			{
				Console.WriteLine(Person.Name + " => " + Person.Single);
			}
		}
	}
}
