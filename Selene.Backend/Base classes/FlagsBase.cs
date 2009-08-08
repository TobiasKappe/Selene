using System;
using System.Text;
using System.Collections.Generic;

namespace Selene.Backend
{
    public abstract class FlagsBase<WidgetType> : EnumBase<WidgetType>
    {
        protected internal override void DetermineIndex (Enum Intermediate)
        {
            CurrentIndex = Convert.ToInt32(Intermediate);
        }

        protected internal override string CurrentName {
            get
            {
                StringBuilder SelectedNames = new StringBuilder();

                foreach(int Index in SelectedIndices)
                {
                    if(SelectedNames.Length > 0) SelectedNames.Append(", ");
                    SelectedNames.Append(Names[Index]);
                }

                return SelectedNames.ToString();
            }
        }

        protected sealed override int CurrentIndex {
            get
            {
                int ret = 0;
                foreach(int Index in SelectedIndices)
                    ret |= Values[Index];

                return ret;
            }
            set
            {
                for(int i = 0; i < Values.Length; i++)
                {
                    ChangeIndex(i, (Values[i] & value) != 0);
                }
            }
        }

        protected abstract void ChangeIndex(int Index, bool Selected);
        protected abstract IEnumerable<int> SelectedIndices { get; }
    }
}
