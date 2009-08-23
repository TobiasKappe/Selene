using System;
using Selene.Backend;
using NUnit.Framework;

#if GTK
using Selene.Gtk.Frontend;
#endif

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

using Qyoto;
using Gtk;

namespace Selene.Testing
{
    public partial class Harness
    {
        class EmbeddingTest
        {
            #pragma warning disable 0649
            
            [Control("Optimus")]
            public string Alpha;
            public bool Bravo;

            [Control("Prime")]
            public ushort[] Color = new ushort[3] { 65535, 0, 0 };
        }

        public void Embedding()
        {
            var Embed = new NotebookDialog<EmbeddingTest>("Selene");
            var Test = new EmbeddingTest();

#if GTK
            Window Container = new Window("Selene");
            Label Extra = new Label("This label is not part of the notebook");
            Button Click = new Button("Neither is this button");
            VBox Box = new VBox();

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
#endif
#if QYOTO
            QDialog Dialog = new QDialog();
            QVBoxLayout MainLay = new QVBoxLayout();
            QHBoxLayout Lay = (QHBoxLayout) Embed.Content(Test);
            MainLay.AddLayout(Lay);
            Dialog.SetLayout(MainLay);
            Dialog.SetWindowTitle("Selene");

            QLabel Label = new QLabel("This label is not part of the notebook");
            QPushButton Button = new QPushButton("Neither is this button");
            MainLay.AddWidget(Label);
            MainLay.AddWidget(Button);

            QWidget.Connect(Button, Qt.SIGNAL("clicked()"), delegate {
                Embed.Save();
                Console.WriteLine(Test.Alpha);
            });

            Dialog.Exec();
#endif
        }
    }
}
