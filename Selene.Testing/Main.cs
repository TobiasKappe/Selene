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
        public static void Main(string[] Args)
        {   
            ITest Execute = new Grouping();
#if QYOTO
            new QApplication(Args);
            Execute.Run();
            QApplication.Exec();
#endif
            
#if GTK
            Application.Init();
            Execute.Run();
            Application.Run();
#endif
        }
    } 
} 