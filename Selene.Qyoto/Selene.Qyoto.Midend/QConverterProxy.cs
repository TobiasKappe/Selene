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
            return string.Empty;
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
            Widg.Disconnect(Qt.SIGNAL(SignalForType(Orig.SubType)));
        }

        void HandleWidgetEvent()
        {
            if(CachedHandlers != null) CachedHandlers(null, null);
        }

        public sealed override event EventHandler Changed {
            add
            {
                if(!Connected) QWidget.Connect(Widg, Qt.SIGNAL(SignalForType(Orig.SubType)), HandleWidgetEvent);
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
