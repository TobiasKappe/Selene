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

using NUnit.Framework;

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

#if GTK
using Selene.Gtk.Frontend;
#endif

#if WINDOWS
using Selene.Winforms.Frontend;
#endif

namespace Selene.Testing
{
    /* "Simplest" test - fill in the values as hinted in the title of the
     * dialog and press OK. Tests saving by converters.
     */
    
    public partial class Harness
    {
        enum Sex { Male, Female }

        class Test
        {
            #pragma warning disable 0649
            
            public string Surname;
            public Sex Gender;
            public bool Newsletter;
        }

        [Test]
        public void Simplest()
        {
            var Present = new NotebookDialog<Test>("Doe, Female, Yes");
            var SaveTo = new Test();
            Assert.IsTrue(Present.Run(SaveTo));
            Assert.AreEqual("Doe", SaveTo.Surname);
            Assert.AreEqual(Sex.Female, SaveTo.Gender);
            Assert.IsTrue(SaveTo.Newsletter);
        }
    }
}
