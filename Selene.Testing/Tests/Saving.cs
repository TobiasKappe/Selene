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
    /* Saving test - checks to see whether the converters properly return
     * the value in the widget, and whether the backend then saves it in 
     * the right field/property on the object. Fill in the answers as the
     * test expects them below, and press OK.
     */
    
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

            Assert.AreEqual("wuppertahl", Test.Wuppertahl.ToLower());
            Assert.AreEqual(42, Test.Answer);
            Assert.AreEqual(1, Test.Newyear.DayOfYear);
            Assert.IsTrue(Test.Check);

            Assert.AreEqual("/etc/hosts", Test.Hosts);
            Assert.AreEqual(Direction.Back, Test.Back);
            Assert.AreEqual(new ushort[] { 0, 0, 0 }, Test.Black);
            Assert.AreEqual(Fruit.Apple | Fruit.Orange, Test.Fr);
        }
    }
}
