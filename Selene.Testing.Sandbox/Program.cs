using System;
using System.Windows.Forms;
using Selene.Winforms.Frontend;

namespace Selene.Testing.Sandbox
{
    class Program
    {
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            NotebookDialog<Test> T = new NotebookDialog<Test>("Title here");
            Test Store = new Test();
            T.Run(Store);

            Console.WriteLine(Store.Toggle);
            Console.WriteLine(Store.Entry);
            Console.WriteLine(Store.Color);
        }

    }
}
