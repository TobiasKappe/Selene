
using System;

namespace Selene.Backend
{
    // Lolcats ahoy!
    public interface IHasUnderlying : IConverter
    {
        void SetUnderlying(Type Given);
    }
}
