
using System;

namespace Selene.Backend
{
    // Lolcats ahoy!
    public interface IHasUnderlying<WidgetType> : IConverter<WidgetType>
    {
        Type Underlying { set; }
    }
}
