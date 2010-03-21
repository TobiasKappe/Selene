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
    /* Numbers test - the dialog presented will sport five different
     * controls:
     *  - The first will be a spinbutton filled with the value '42' No 
     *    limits should be imposed on this spinbutton. 
     *  - The second will be a spinbutton with a minimum value of 0 and
     *    a maximum value of 10. 
     *  - The third widget will be a spinbutton, again. This spinbutton
     *    is wrapped, meaning it will jump from the maximum value to the 
     *    minimum and vice-versa, if pushed over the limit. 
     *  - The fourth will be a spinbutton with the value of '2', but 
     *    configured to take steps of 5 between 0 and 50. Note that this
     *    one is also wrapped.
     *  - The fifth and sixth widgets will be gliders, respectively 
     *    horizontal and vertical. These are bound from 0 to 10, but
     *    should show no value.
     * 
     * If all above behave appropriately, press OK.
     */
    
    public partial class Harness
    {
        class NumberTest
        {
            #pragma warning disable 0649

            public int Answer = 42;

            [ControlFlags(0, 10)]
            public int Bound = 7;

            [ControlFlags(0, 1, true)]
            public int Wrapped = 1;

            [ControlFlags(0, 50, 5, true)]
            public int Stepped = 2;
            
            [Control(Override = ControlType.Glider), ControlFlags(0, 10)]
            public int Horizontal;
            
            [Control(Override = ControlType.Glider), ControlFlags(true, 0, 10)]
            public int Vertical;
        }

        [Test]
        public void Numbers()
        {
            NotebookDialog<NumberTest> Test = new NotebookDialog<NumberTest>("Correct entries?");
            var Store = new NumberTest();
            Assert.IsTrue(Test.Run(Store));
        }
    }
}
