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
using Qyoto;

namespace Selene.Qyoto.Midend
{
    delegate string ResolveSignal(ControlType Type);

    /* The role of this class is twofold:
     * - As a base class for "normal converters" (using protected constructor)
     * - As a field in EnumChooser/ListViewer (using public constructor)
     */

    public class QConverterProxy<T> : ConverterBase<QObject, T>
    {
        internal ResolveSignal Resolve;

        event EventHandler CachedHandlers;

        bool IsParent = true;
        bool Connected = false;
        Control mOrig;
        QObject mWidg;

        protected Control Orig {
            get
            {
                if(IsParent) return base.Original;
                else return mOrig;
            }
        }

        internal QObject Widg {
            get
            {
                if(IsParent) return base.Widget;
                else return mWidg;
            }
            set { mWidg = value; }
        }

        protected virtual string SignalForType(ControlType Type)
        {
            if(Resolve != null) return Resolve(Type);
            return null;
        }

        public QConverterProxy(Control Orig, QObject Widg) : this()
        {
            IsParent = false;
            mOrig = Orig;
            mWidg = Widg;
        }

        protected QConverterProxy()
        {
        }

        ~QConverterProxy()
        {
            string signal = SignalForType(Orig.SubType);

            if(signal != null)
                Widg.Disconnect(Qt.SIGNAL(signal));
        }

        void HandleWidgetEvent()
        {
            FireChanged();
        }

        protected void FireChanged()
        {
            if(CachedHandlers != null) CachedHandlers(Widg, null);
        }

        public sealed override event EventHandler Changed {
            add
            {
                string signal = SignalForType(Orig.SubType);

                if(signal != null && !Connected)
                    QWidget.Connect(Widg, Qt.SIGNAL(signal), HandleWidgetEvent);

                CachedHandlers += value;
            }
            remove { CachedHandlers -= value; }
        }

        // Must be overridden by inheritor and not called by using class
        protected override T ActualValue {
            get {
                throw new System.NotImplementedException ();
            }
            set {
                throw new System.NotImplementedException ();
            }
        }

        protected override QObject Construct ()
        {
            throw new System.NotImplementedException ();
        }
    }
}
