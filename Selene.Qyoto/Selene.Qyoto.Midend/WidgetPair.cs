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

        public WidgetPair(Control Original)
        {
            this.Flags = Original.Flags;
            this.Info = Original.Info;
            this.Label = Original.Label;
            this.SubType = Original.SubType;
        }

        public WidgetPair()
        {
        }
    }
}
