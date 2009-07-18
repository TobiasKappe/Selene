
using System;

namespace Selene.Backend
{
    public interface IModalPresenter : IPresenter
    {
        bool Run<T>(T Presented);
    }
}
