using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class DateTimeEdit : QConverterProxy<DateTime>
    {
        protected override DateTime ActualValue {
            get
            {
                QDateTimeEdit Edit = Widget as QDateTimeEdit;
                return new DateTime(Edit.Date.Year(), Edit.Date.Month(), Edit.Date.Day(),
                                    Edit.Time.Hour(), Edit.Time.Minute(), Edit.Time.Second(), Edit.Time.Msec());
            }
            set
            {
                QDateTimeEdit Edit = Widget as QDateTimeEdit;
                Edit.SetDate( new QDate(value.Year, value.Month, value.Day) );
                Edit.SetTime( new QTime(value.Hour, value.Minute, value.Second, value.Millisecond) );
            }
        }

        protected override QObject Construct ()
        {
            var Ret = new QDateTimeEdit();

            string Format = Original.GetFlag<string>();

            if(Format != null)
                Ret.DisplayFormat = Format;

            return Ret;
        }

        protected override string SignalForType (ControlType Type)
        {
            return "dateTimeChanged(QDateTime)";
        }

    }
}
