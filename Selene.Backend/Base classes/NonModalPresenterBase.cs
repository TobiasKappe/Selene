// Copyright (c) 2009 Tobias Kappé
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// Except as contained in this notice, the name(s) of the above
// copyright holders shall not be used in advertising or otherwise
// to promote the sale, use or other dealings in this Software
// without prior written authorization.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

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
