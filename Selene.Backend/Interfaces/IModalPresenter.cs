
using System;

namespace Selene.Backend
{
    public interface IModalPresenter<T> : IPresenter<T>
    {
        bool Run(T Presented);
    }
}
