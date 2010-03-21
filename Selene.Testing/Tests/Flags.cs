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

#if GTK
using Selene.Gtk.Frontend;
#endif

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

#if WINDOWS
using Selene.Winforms.Frontend;
#endif

using Selene.Backend;

namespace Selene.Testing
{
    /* Flags test - tests the implementation of FlagsBase given in the
     * frontend. The user should check "apple" and "banana" and press OK.
     * 
     * TODO: The last clicked item gets painted unselected on the windows
     * frontend as soon as the user clicks outside the widget. To the
     * toolkit, however, this widget still appears selected. Should 
     * probably file a bug with mono for this.
     */
    
    public partial class Harness
    {
        class FlagsTest
        {
            #pragma warning disable 0649
            
            public Fruit Choose;
            
            [Control(Override = ControlType.MultiSelect)]
            public Fruit Choose2;
        }

        [Test]
        public void Flags()
        {
            var Test = new NotebookDialog<FlagsTest>("Which is good?");
            FlagsTest Result = new FlagsTest();
            
            Assert.IsTrue(Test.Run(Result));
            Assert.AreEqual(Fruit.Apple | Fruit.Banana, Result.Choose);
            Assert.AreEqual(Fruit.Apple | Fruit.Banana, Result.Choose2);
        }
    }
}
