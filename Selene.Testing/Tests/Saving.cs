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
#endif

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

#if WINDOWS
using Selene.Winforms.Frontend;
#endif

namespace Selene.Testing
{
    public partial class Harness
    {
        // Used in filling test too
        class MultiTest
        {
            #pragma warning disable 0649

            public string Wuppertahl;
            public int Answer;
            public DateTime Newyear = System.DateTime.Now;
            public bool Check;

            [Control(Override = ControlType.FileSelect)]
            public string Hosts;

            public Direction Back;
            public ushort[] Black = new ushort[] { 0, ushort.MaxValue, 0 };

            [Control(Name = "Apple, orange")]
            public Fruit Fr;
        }

        [Test]
        public void Saving()
        {
            NotebookDialog<MultiTest> Saver = new NotebookDialog<MultiTest>("Do comply");

            var Test = new MultiTest();

            Saver.Run(Test);

            Assert.AreEqual(Test.Wuppertahl.ToLower(), "wuppertahl");
            Assert.AreEqual(Test.Answer, 42);
            Assert.AreEqual(Test.Newyear.DayOfYear, 1);
            Assert.IsTrue(Test.Check);

            Assert.AreEqual(Test.Hosts, "/etc/hosts");
            Assert.AreEqual(Test.Back, Direction.Back);
            Assert.AreEqual(Test.Black, new ushort[] { 0, 0, 0 });
            Assert.AreEqual(Test.Fr, Fruit.Apple | Fruit.Orange);
        }
    }
}
