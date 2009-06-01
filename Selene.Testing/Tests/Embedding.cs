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
    public class Embedding : ITest
    {
        EmbeddingTest Test;
        NotebookDialog<EmbeddingTest> Embed;
        
        public class EmbeddingTest
        {
            [Control("Optimus")]
            public string Alpha;
            public bool Bravo;
            
            [Control("Prime")]
            public ushort[] Color = new ushort[3] { 65535, 0, 0 };
        }
        
        void Clicked(object sender, EventArgs Args)
        {
            Embed.Save();
            Console.WriteLine(Test.Alpha);
        }
        
        public void Run()
        {
#if GTK
            Window Container = new Window("Selene");
            Label Extra = new Label("This label is not part of the notebook");
            Button Click = new Button("Neither is this button");
            Test = new EmbeddingTest();
            VBox Box = new VBox();
            Embed = new NotebookDialog<EmbeddingTest>("Selene");
            
            Click.Clicked += Clicked;
            Box.Add(Embed.Content(Test));
            Box.Add(Extra);
            Box.Add(Click);
            Container.BorderWidth = 5;
            Box.Spacing = 5;
            Container.Add(Box);
            Container.ShowAll();
#endif
        }
    }
}
