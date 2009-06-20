
using System;

namespace Selene.Backend
{   
    public interface IPresenter<T>
    {
        void Save();
        void Show();
        void Hide();
    }
}
