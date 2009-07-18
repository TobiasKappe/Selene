using System;
using System.Reflection;

namespace Selene.Backend
{
    public abstract class NonModalPresenterBase<WidgetType> : DisplayBase<WidgetType>, INonModalPresenter
    {
        static NonModalPresenterBase()
        {
            CacheConverters(Assembly.GetCallingAssembly());
        }

        public event Done Finished;

        public bool Success {
            get {
                if(Done == null)
                    throw new Exception("The presenter has not run yet.");

                return Done.Value;
            }
            protected set {
                Done = value;
                if(Finished != null) Finished(Done.Value);
            }
        }

        protected bool? Done = null;

        public void Run<T>(T Present)
        {
            Prepare(typeof(T), Present);

            Run();
        }

        protected abstract void Run();
    }
}
