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
    /* DateTime test - checks to see whether the custom flags provided
     * by the Gtk and Qt/Qyoto frontends are displayed properly. To do
     * so, we try to derive from the default settings as much as possible.
     * 
     * The Gtk frontend should show:
     *  - No heading 
     *  - No day names
     *  - No month change
     *  - Week numbers
     *  - Monday not as the first day of the week
     * The latter condition may or may not apply depending on the GTK
     * settings that come with your operating system.
     * 
     * The Qt/Qyoto frontend should show a formatted date.
     * 
     * To pass this test, click "OK" if above conditions apply.
     */
    
    public partial class Harness
    {
#if !WINDOWS
        class DateTimeTest
        {
            #pragma warning disable 0649

#if QYOTO
            [ControlFlags("dd.MM.yyyy")]
#endif
#if GTK
            [ControlFlags(false, false, true, true, false)]
#endif
            public DateTime Now = System.DateTime.Now;
        }

        [Test]
        public void DateTime()
        {
#if QYOTO
            const string Title = "Formatted date";
#endif
#if GTK
            const string Title = "Changed calendar";
#endif
            var Dialog = new NotebookDialog<DateTimeTest>(Title);
            Assert.IsTrue(Dialog.Run(new DateTimeTest()));
        }
#endif
    }
}
