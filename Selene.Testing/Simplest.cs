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
	public class Simplest
	{
		public enum Sex { Male, Female } 
     
	    class Test 
	    {                        
	        public string Surname; 
	        public Sex Gender; 
	        public bool Newsletter;       
	    }
		
        public static void Show() 
        { 
            var Present = new NotebookDialog<Test>("Selene"); 
            var SaveTo = new Test(); 
            Present.Run(SaveTo); 
             
            Console.WriteLine(SaveTo.Surname); 
            Console.WriteLine(SaveTo.Gender); 
            Console.WriteLine(SaveTo.Newsletter); 
        } 
	}
}
