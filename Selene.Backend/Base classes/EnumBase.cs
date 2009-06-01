using System;
using System.Collections.Generic;

namespace Selene.Backend
{
    public abstract class EnumBase : IHasUnderlying
    {
        private string[] Values;
        private Type Actual;
        
        public Type Type { get { return typeof(Enum); } }

        public void SetValue (Control Original, object Value)
        {
            int i;
            for(i = 0; i < Values.Length; i++)
            {
                if(Value.ToString() == Values[i]) break;
            }
            
            if(i != Values.Length) SetIndex(Original, i);
        }

        public object ToObject (Control Start)
        {
            return Enum.Parse(Actual, Values[GetIndex(Start)]);
        }

        public void SetUnderlying(Type Given)
        {
            Values = Enum.GetNames(Actual = Given);
        }
        
        public Control ToWidget(Control Start)
        {
            List<string> Labels = new List<string>();

            for(int i = 0; i < Values.Length; i++)
                Labels.Add(Values[i]);

            Control obj = ToWidget(Start, Labels.ToArray());

            return obj;
        }
        
        protected abstract void SetIndex(Control Original, int Index);
        protected abstract int GetIndex(Control Original);
        protected abstract Control ToWidget(Control Start, string[] Values);
    }
}
