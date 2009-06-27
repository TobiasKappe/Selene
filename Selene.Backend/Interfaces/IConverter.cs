
using System;

namespace Selene.Backend
{
    public interface IConverter<WidgetType>
    {
        Type Type { get; }
        Control Primitive { get; }

        WidgetType Construct(Control Original);

        object Value { get; set; }
        event EventHandler Changed;
    }
}
