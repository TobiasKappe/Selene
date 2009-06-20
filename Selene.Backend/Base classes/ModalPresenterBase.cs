using System;
using System.Reflection;

namespace Selene.Backend
{
    public abstract class ModalPresenterBase<T> : DisplayBase<T>, IModalPresenter<T> where T : class
    {
        static ModalPresenterBase()
        {
            CacheConverters(Assembly.GetCallingAssembly());
        }

        protected ModalPresenterBase()
        {
        }

        public virtual bool Run(T Present)
        {
            Prepare(Present);
            Show();

            return true;
        }
    }
}
