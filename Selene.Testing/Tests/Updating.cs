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
using Selene.Backend;

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
    /* Update test - check whether the Update() method on DisplayBase
     * works as intended and updates the fields. To test, fill in "teh",
     * it should be changed to "the".
     */
    
    public partial class Harness
    {
        class UpdateTest
        {
            #pragma warning disable 0649
            public string DefiniteArticle;
        }

        [Test]
        public void Update()
        {
            var Present = new NotebookDialog<UpdateTest>("Fill in definitive article");
            var Save = new UpdateTest();
            
            Present.SubscribeAllChange<UpdateTest>(delegate { 
                Present.Save();
                if(Save.DefiniteArticle == "teh")
                {
                    /* Be careful calling Update() within a change listener, because
                     * an update will trigger at least one change too, calling this
                     * method again. Stack overflows are easy to get this way */
                    Save.DefiniteArticle = "the";
                    Present.Update();
                }
            });
            
            Assert.IsTrue(Present.Run(Save));
        }
    }
}
