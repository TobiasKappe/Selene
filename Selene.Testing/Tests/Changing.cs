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
using System.Collections.Generic;
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
    /* Changing test - tests to see whether the appropriate event is fired
     * by each converter. It counts the events fired by making use of
     * SubscribeAllChange<T>(). Note that the amount of events may differ
     * per frontend, as some events fired by initializing the widgets
     * could not be avoided.
     * 
     * To pass this test properly, make one (no more, no less) change to
     * each widget presented in the window.
     */
    
    public partial class Harness
    {
        [Flags]
        enum Fruit { Apple = 1, Orange = 2, Banana = 4 }
        enum Direction { Left, Right, Back }
        
        List<object> Marked = new List<object>();
        NotebookDialog<Change> Dialog;

        class Change
        {
            #pragma warning disable 0649

            // Simple widgets
            public bool Check;
            public string Enter;
            [Control(Override = ControlType.FileSelect)]
            public string File;
            public int Number;

            // Enum-based widgets
            public Fruit Bowl = Fruit.Orange | Fruit.Apple;
            [Control(Override = ControlType.Radio)]
            public Direction Choose;
            public Direction Choose2;

            // Compound widgets
            public DateTime Calendar;
            public ushort[] Color = new ushort[] { ushort.MaxValue, 0, 0 };
            public Enclosed[] List = new Enclosed[] {};
        }

        int TimesChanged = 0;

        [Test]
        public void Changing()
        {
            Dialog = new NotebookDialog<Change>("Selene");
            Dialog.SubscribeAllChange<Change>(HandleChange);
            
            Assert.IsTrue(Dialog.Run(new Change()));
            Assert.AreEqual(10, TimesChanged);
        }

        void HandleChange(object Sender, EventArgs Args)
        {
            if(!Dialog.Visible) return;
            if(Marked.Contains(Sender)) return;
            
            TimesChanged++;
            Marked.Add(Sender);
        }
    }
}
