
using System;

namespace Selene.Backend
{   
    public interface IEmbeddable<TWidget, TSave>
    {
        TWidget Content(TSave Present);
        bool IsEmbedded { get; }
    }
}
