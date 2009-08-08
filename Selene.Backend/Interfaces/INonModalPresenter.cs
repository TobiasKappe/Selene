
using System;

namespace Selene.Backend
{
    public delegate void Done(bool Success);

    public interface INonModalPresenter : IPresenter
    {
        event Done Finished;
        void Run<T>(T Present);

        bool Success { get; }
        void Block();
    }
}
