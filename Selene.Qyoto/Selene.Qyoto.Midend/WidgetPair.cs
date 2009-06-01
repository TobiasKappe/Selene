using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class WidgetPair : Control
    {
        public QObject Widget;
        public QLabel LabelWidget;
        public bool HasLabel = true;
    }
}
