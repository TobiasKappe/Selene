using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class StringEntry : ConverterBase<WidgetPair, string>
    {
        // We do not "support" these modes but default to entry for all
        public StringEntry() : base(ControlType.Entry, ControlType.FileSelect, ControlType.DirectorySelect)
        {
        }
        
        protected override WidgetPair ToWidget (WidgetPair Original, string Value)
        {
            QLineEdit Entry = new QLineEdit(Value);
            Original.Widget = Entry;
            
            return Original;
        }
        
        protected override string ToValue (WidgetPair Start)
        {
            return (Start.Widget as QLineEdit).Text;
        }
        
        protected override void SetValue (WidgetPair Original, string Value)
        {
            (Original.Widget as QLineEdit).Text = Value;
        }
    }
}
