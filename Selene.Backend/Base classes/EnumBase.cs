using System;
using System.Collections.Generic;

namespace Selene.Backend
{
    public abstract class EnumBase<TController> : IConverter where TController: Control, new()
    {
        private string[] Values;
        private Type Actual;
        
        public Type Type { get { return typeof(Enum); } set { }}
        
        private void SetValues(Enum Value)
        {
            if(Values == null)
            {
                Actual = Value.GetType();
                Values = Enum.GetNames(Actual);
            }
        }
        
        public void SetValue (Control Original, object Value)
        {
            SetValues(Value as Enum);
            
            int i;
            for(i = 0; i < Values.Length; i++)
            {
                if(Value.ToString() == Values[i]) break;
            }
            
            if(i != Values.Length) SetIndex(Original as TController, i);
        }

        public object ToObject (Control Start)
        {
            return Enum.Parse(Actual, Values[GetIndex(Start as TController)]);
        }
        
        public Control ToWidget(Control Start, object Value)
        {
            SetValues(Value as Enum);
            
            List<string> Labels = new List<string>();
            int Active = 0, i;
            
            for(i = 0; i < Values.Length; i++)
            {
                Labels.Add(Values[i]);
                if(Values[i] != null && Value.ToString() == Values[i]) 
                    Active = i;
            }
            
            Control obj = ToWidget(Start.Subclassify<TController>(), Labels.ToArray());
            SetValue(obj, Value);
            
            return obj;
        }
        
        protected abstract void SetIndex(TController Original, int Index);
        protected abstract int GetIndex(TController Original);
        protected abstract Control ToWidget(TController Start, string[] Values);
    }
}
