using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class DateTimeCalendar : ConverterBase<Widget, DateTime>
    {
        protected override DateTime ActualValue {
            get
            {
                return (Widget as Calendar).Date;
            }
            set
            {
                (Widget as Calendar).Date = value;
            }
        }

        protected override Widget Construct ()
        {
            return new Calendar();
        }

        public override event EventHandler Changed {
            add
            {
                (Widget as Calendar).DaySelected += value;
            }
            remove
            {
                (Widget as Calendar).DaySelected -= value;
            }
        }
    }
}
