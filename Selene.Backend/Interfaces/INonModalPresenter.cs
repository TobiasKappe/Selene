
using System;

namespace Selene.Backend
{
    public delegate void Done(bool Success);

    public interface INonModalPresenter<T> : IPresenter<T>
    {
        event Done OnDone;
        void Run(T Present);

        bool Success { get; }
    }
}
