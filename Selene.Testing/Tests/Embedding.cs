#if GTK
using NUnit.Framework;
using Selene.Backend;

using System;
using Gtk;

using Selene.Gtk.Frontend;

namespace Selene.Testing
{
    public partial class Harness
    {
        class EmbeddingTest
        {
            [Control("Optimus")]
            public string Alpha;
            public bool Bravo;

            [Control("Prime")]
            public ushort[] Color = new ushort[3] { 65535, 0, 0 };
        }

        public void Embedding()
        {
            Window Container = new Window("Selene");
            Label Extra = new Label("This label is not part of the notebook");
            Button Click = new Button("Neither is this button");
            var Test = new EmbeddingTest();
            VBox Box = new VBox();
            var Embed = new NotebookDialog<EmbeddingTest>("Selene");

            Click.Clicked += delegate {
                Embed.Save();
                Console.WriteLine(Test.Alpha);
            };
            Box.Add(Embed.Content(Test));
            Box.Add(Extra);
            Box.Add(Click);
            Container.BorderWidth = 5;
            Box.Spacing = 5;
            Container.Add(Box);
            Container.ShowAll();
        }
    }
}
#endif
