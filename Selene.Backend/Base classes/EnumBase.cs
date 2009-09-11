using System;
using System.Collections.Generic;

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
