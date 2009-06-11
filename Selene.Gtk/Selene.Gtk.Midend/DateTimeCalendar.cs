using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{   
    public class DateTimeCalendar : ConverterBase<WidgetPair, DateTime>
    {
        protected override DateTime ToValue (WidgetPair Start)
        {
            return (Start.Widget as Calendar).Date;
        }
        
        protected override void SetValue (WidgetPair Original, DateTime Value)
        {
            (Original.Widget as Calendar).Date = Value;
        }

        protected override WidgetPair ToWidget (WidgetPair Original)
        {
            Calendar Cal = new Calendar();
            Original.Widget = Cal;

            return Original;
        }

        protected override void ConnectChange (WidgetPair Original, EventHandler OnChange)
        {
            (Original.Widget as Calendar).DaySelected += OnChange;
        }
    }
}
