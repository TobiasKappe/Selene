
using System;
using Gtk;

namespace Selene.Testing
{
    public class Setup
    {   
        public static void TkSetup()
        {
#if GTK
            string[] Dummy = new string[] { };
            Init.Check(ref Dummy);
#endif
        }
    }
}
