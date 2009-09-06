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
            var Ret = new Calendar();

            // Attempt to interfere as little as possible with the i18n
            CheckFlag(Ret, 0, CalendarDisplayOptions.ShowHeading);
            CheckFlag(Ret, 1, CalendarDisplayOptions.ShowDayNames);
            CheckFlag(Ret, 2, CalendarDisplayOptions.NoMonthChange);
            CheckFlag(Ret, 3, CalendarDisplayOptions.ShowWeekNumbers);
            CheckFlag(Ret, 4, CalendarDisplayOptions.WeekStartMonday);

            return Ret;
        }

        void CheckFlag(Calendar Cal, int Index, CalendarDisplayOptions Option)
        {
            bool Value = false;
            if(Original.GetFlag<bool>(Index, ref Value))
            {
                if(Value) Cal.DisplayOptions |= Option;
                else Cal.DisplayOptions &= ~Option;
            }
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
