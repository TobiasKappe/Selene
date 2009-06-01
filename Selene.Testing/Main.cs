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
        public delegate void Show();

        public static void Main(string[] Args)
        {
            Harness Testing = new Harness();
            Show Execute = Testing.Simplest;
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