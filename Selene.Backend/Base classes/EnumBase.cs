using System;
using System.Collections.Generic;

namespace Selene.Backend
{
    public abstract class EnumBase<WidgetType> : ConverterBase<WidgetType, Enum>, IHasUnderlying<WidgetType>
    {
        string[] Names;
        int[] Values;
        Type mUnderlying;

        public Type Underlying {
            set { mUnderlying = value; }
        }

        protected sealed override Enum ActualValue {
            get
            {
                return Enum.Parse(mUnderlying, Names[CurrentIndex]) as Enum;
            }
            set
            {
                PrepareOptions();

                for(int i = 0; i < Values.Length; i++)
                {
                    if(Values[i] == Convert.ToInt32(value))
                        CurrentIndex = i;
                }
            }
        }

        void PrepareOptions()
        {
            if(Names != null) return;

            Names = Enum.GetNames(mUnderlying);
            Values = Enum.GetValues(mUnderlying) as int[];

            foreach(string Name in Names) AddOption(Name);
        }

        protected abstract void AddOption(string Value);
        protected abstract int CurrentIndex { get; set; }
    }
}
