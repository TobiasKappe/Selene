using System;
using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class DateTimeEdit : ConverterBase<WidgetPair, DateTime>
    {
        protected override void SetValue (WidgetPair Original, DateTime Value)
        {
            QDateTimeEdit Edit = Original.Widget as QDateTimeEdit;
            Edit.SetDate( new QDate(Value.Year, Value.Month, Value.Day) );
            Edit.SetTime( new QTime(Value.Hour, Value.Minute, Value.Second, Value.Millisecond) );
        }
        
        protected override DateTime ToValue (WidgetPair Start)
        {
            QDateTimeEdit Edit = Start.Widget as QDateTimeEdit;
            DateTime Ret = new DateTime(Edit.Date.Year(), Edit.Date.Month(), Edit.Date.Day(),
                                        Edit.Time.Hour(), Edit.Time.Minute(), Edit.Time.Second(), Edit.Time.Msec());
            return Ret;
        }
        
        protected override WidgetPair ToWidget (WidgetPair Original, DateTime Value)
        {
            QDateTimeEdit Edit = new QDateTimeEdit();
            Original.Widget = Edit;
            SetValue(Original, Value);
            
            return Original;
        }
    }
}
