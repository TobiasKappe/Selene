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
	[TestFixture]
	public class ListView : ITest
	{	
		public class Container
		{
			[ControlFlags("Person dialog")]
			public Enclosed[] People = new Enclosed[] { 
				new Enclosed { Name = "Lisa Cuddy", Single = true } };
		}
		
		public class Enclosed
		{
			public string Name;
			public bool Single;
		}
		
		[Test]
		public void Run()
		{
			var Disp = new NotebookDialog<Container>("Add one person");
			var Test = new Container();
			
			Assert.IsTrue(Disp.Run(Test));
			Assert.AreEqual(Test.People.Length, 2);
		}
	}
}
