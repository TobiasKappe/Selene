using System;
using System.Reflection;

namespace Selene.Backend
{
    public abstract class ModalPresenterBase<WidgetType> : DisplayBase<WidgetType>, IModalPresenter
    {
        static ModalPresenterBase()
        {
            CacheConverters(Assembly.GetCallingAssembly());
        }

        public bool Run<T>(T Present)
        {
            return Run(typeof(T), Present);
        }

        // Used by ListViewerBase
        internal bool Run(Type T, object Present)
        {
            Prepare(T, Present);
            return Run();
        }

        protected abstract bool Run();
    }
}
