using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class BoolToggler : ConverterBase<WidgetPair, bool>
    {
        protected override void SetValue (WidgetPair Original, bool Value)
        {
            (Original.Widget as QCheckBox).Checked = Value;
        }

        protected override bool ToValue (WidgetPair Start)
        {
            return (Start.Widget as QCheckBox).Checked;
        }

        protected override WidgetPair ToWidget (WidgetPair Original, bool Value)
        {
            QCheckBox Box = new QCheckBox(Original.Label);
            Original.Widget = Box;
            Original.HasLabel = false;
            return Original;
        }
    }
}
