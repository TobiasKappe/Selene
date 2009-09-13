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

namespace Selene.Backend
{
    public abstract class EnumBase<WidgetType> : ConverterBase<WidgetType, Enum>, IHasUnderlying<WidgetType>
    {
        protected internal string[] Names;
        protected internal int[] Values;
        protected internal Type mUnderlying;

        public Type Underlying {
            set { mUnderlying = value; }
            get { return mUnderlying; }
        }

        protected internal virtual string CurrentName
        {
            get { return Names[CurrentIndex]; }
        }

        protected internal virtual void DetermineIndex(Enum Intermediate)
        {
            for(int i = 0; i < Values.Length; i++)
            {
                if(Values[i] == Convert.ToInt32(Intermediate))
                    CurrentIndex = i;
            }
        }

        protected sealed override Enum ActualValue {
            get
            {
                string Name = CurrentName;

                if(Name == string.Empty)
                    return null;

                return Enum.Parse(mUnderlying, CurrentName) as Enum;
            }
            set
            {
                PrepareOptions();
                DetermineIndex(value);
            }
        }

        void PrepareOptions()
        {
            if(Names != null) return;

            Names = Enum.GetNames(mUnderlying);
            Values = Enum.GetValues(mUnderlying) as int[];

            foreach(string Name in Names) AddOption(Name);

            CurrentIndex = 0;
        }

        protected abstract void AddOption(string Value);
        protected abstract int CurrentIndex { get; set; }
    }
}
