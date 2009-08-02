using System;
using System.Reflection;
using System.Threading;

namespace Selene.Backend
{
    public abstract class NonModalPresenterBase<WidgetType> : DisplayBase<WidgetType>, INonModalPresenter
    {
        static NonModalPresenterBase()
        {
            CacheConverters(Assembly.GetCallingAssembly());
        }

        public event Done Finished;
        protected bool? Done = null;

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

        public void Run<T>(T Present)
        {
            Prepare(typeof(T), Present);

            Run();
        }

        protected abstract void Run();
        public abstract void Block();
    }
}
