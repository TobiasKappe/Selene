using System;
using System.Reflection;

namespace Selene.Backend
{
    public abstract class ModalPresenterBase<WidgetType, T> : DisplayBase<WidgetType, T>, IModalPresenter<T> where T : class
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
