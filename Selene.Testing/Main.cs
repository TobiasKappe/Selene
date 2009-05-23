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
    public class Demo
    {
		private delegate void Show();
		
		public static void Main(string[] Args)
		{	
			Show Execute = Simplest.Show;
#if QYOTO
			new QApplication(Args);
			Execute();
			QApplication.Exec();
#endif
			
#if GTK
			Application.Init();
			Execute();
			Application.Run();
#endif
		}
    } 
} 