
using System;

namespace Selene.Backend
{
    public interface IConverter
    {
        Type Type { get; }
        
        Control ToWidget(Control Original);
        object ToObject(Control Start);
        void SetValue(Control Original, object Value);
    }
}
