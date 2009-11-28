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
using Gtk;

namespace Selene.Gtk.Midend
{
    public class DateTimeEdit : ConverterBase<Widget, DateTime>
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
