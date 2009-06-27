using System;
using System.Reflection;

namespace Selene.Backend
{
    public abstract class NonModalPresenterBase<WidgetType, T> : DisplayBase<WidgetType, T>, INonModalPresenter<T> where T : class
    {
        static NonModalPresenterBase()
        {
            CacheConverters(Assembly.GetCallingAssembly());
        }

        public event Done OnDone;

        public bool Success {
            get {
                if(Done == null)
                    throw new Exception("The presenter has not run yet.");

                return Done.Value;
            }
            protected set {
                Done = value;
                if(OnDone != null) OnDone(Done.Value);
            }
        }

        protected bool? Done = null;

        protected NonModalPresenterBase()
        {
        }

        public virtual void Run(T Present)
        {
            Prepare(Present);
            Show();
        }
    }
}
