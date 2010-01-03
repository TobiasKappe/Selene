// Copyright (c) 2009 Tobias Kappé
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// Except as contained in this notice, the name(s) of the above
// copyright holders shall not be used in advertising or otherwise
// to promote the sale, use or other dealings in this Software
// without prior written authorization.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using Selene.Backend;
using NUnit.Framework;

#if GTK
using Selene.Gtk.Frontend;
using Gtk;
#endif

#if QYOTO
using Selene.Qyoto.Frontend;
using Qyoto;
#endif

#if WINDOWS
using Selene.Winforms.Frontend;
using Selene.Winforms.Ordering;
using System.Windows.Forms;
#endif

namespace Selene.Testing
{
    public partial class Harness
    {
        class EmbeddingTest
        {
            #pragma warning disable 0649

            [ControlAttribute("Optimus")]
            public string Alpha;
            public bool Bravo;

            [ControlAttribute("Prime")]
            public ushort[] Color = new ushort[3] { 65535, 0, 0 };
        }

        public void Embedding()
        {
            var Embed = new NotebookDialog<EmbeddingTest>("Selene");
            var Test = new EmbeddingTest();

            const string WindowTitle = "Selene";
            const string LabelText = "This label is not part of the notebook";
            const string ButtonText = "Neither is this button";

#if GTK
            Window Container = new Window(WindowTitle);
            Label Extra = new Label(LabelText);
            Button Click = new Button(ButtonText);
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
            Dialog.SetWindowTitle(WindowTitle);

            QLabel Label = new QLabel(LabelText);
            QPushButton Button = new QPushButton(ButtonText);
            MainLay.AddWidget(Label);
            MainLay.AddWidget(Button);

            QWidget.Connect(Button, Qt.SIGNAL("clicked()"), delegate {
                Embed.Save();
                Console.WriteLine(Test.Alpha);
            });

            Dialog.Exec();
#endif
#if WINDOWS
            Form MainForm = new Form();
            ControlVBox MainBox = new ControlVBox(5,5);
            MainForm.Text = WindowTitle;

            var Content = Embed.Content(Test);
            Content.AutoSize = true;
            MainBox.Controls.Add(Content);

            Label L = new Label();
            L.Text = LabelText;
            L.AutoSize = true;
            MainBox.Controls.Add(L);

            Button B = new Button();
            B.Text = ButtonText;
            B.Click += delegate {
                Embed.Save();
                Console.WriteLine(Test.Alpha);
            };
            MainBox.Controls.Add(B);

            MainForm.Controls.Add(MainBox);

            MainForm.AutoSize = true;
            MainForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MainForm.Show();
#endif
        }
    }
}
