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
	public class Simplest : ITest
	{
		public enum Sex { Male, Female } 
     
	    class Test 
	    {                        
	        public string Surname; 
	        public Sex Gender; 
	        public bool Newsletter;       
	    }
		
		[Test]
		public void Run()
		{
			Setup.TkSetup();
			var Present = new NotebookDialog<Test>("Doe, Male, Yes"); 
            var SaveTo = new Test(); 
            Assert.IsTrue(Present.Run(SaveTo));
			Assert.AreEqual(SaveTo.Surname, "Doe");
			Assert.AreEqual(SaveTo.Gender, Sex.Male);
			Assert.IsTrue(SaveTo.Newsletter);
		}
	}
}
