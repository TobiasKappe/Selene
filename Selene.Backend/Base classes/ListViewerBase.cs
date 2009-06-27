using System;
using System.Collections.Generic;
using System.Reflection;

namespace Selene.Backend
{
    public abstract class ListViewerBase<WidgetType> : ConverterBase<WidgetType, object[]>, IHasUnderlying<WidgetType>
    {
        protected int Counter = 0;
        protected ModalPresenterBase<WidgetType, object> Dialog;

        protected bool AllowsEdit = true, AllowsRemove = true,
            AllowsAdd = true, GreyButtons = false;

        int i = 0;
        List<object> Content;
        ConstructorInfo Constructor;
        bool Inspected = false;
        Type mUnderlying;

        public Type Underlying {
            set
            {
                if((Constructor = value.GetConstructor(Type.EmptyTypes)) == null)
                    throw new InspectionException(value, "Type for list view should contain paramaterless constructor");

                mUnderlying = value;
            }
        }

        #region Manipulation functions
        protected void DeleteRow(int Id)
        {
            Content.RemoveAt(Id);
            i--;
        }

        protected void AddRow()
        {
            object Fill = Constructor.Invoke(null);

            if(!Inspected)
            {
                DisplayBase<WidgetType, object>.ForceInspect(mUnderlying);
                Inspected = true;
            }
            bool Ret = Dialog.Run(Fill);

            if(!Ret) return;
            Content.Add(Fill);
            RowAdded(i++, BreakItDown(Fill));
        }
        
        protected void EditRow(int Id)
        {
            Dialog.Run(Content[Id]);
            RowEdited(Id, BreakItDown(Content[Id]));
        }
        #endregion
        
        #region Auxiliary functions
        private object[] BreakItDown(object Begin)
        {
            return BreakItDown(Begin, Begin.GetType().GetFields());
        }
        
        private object[] BreakItDown(object Begin, FieldInfo[] Infos)
        {
            List<object> Ret = new List<object>();
            if(Begin == null) return null;
            
            foreach(FieldInfo Info in Infos)
            {
                if(!IsViewable(Info.FieldType)) continue;
                Ret.Add(Info.GetValue(Begin));
            }
                        
            return Ret.ToArray();
        }
        #endregion

        #region Interface implementation
        protected sealed override object[] ActualValue {
            set
            {
                foreach(object Item in (object[])value)
                {
                    object[] Columns = BreakItDown(Item);
                    Content.Add(Item);
                    RowAdded(i++, Columns);
                }
            }
            get
            {
                Type ToArray = mUnderlying.MakeArrayType();

                // And now for the ugliest line in C# history:
                object[] GoodType = (object[]) ToArray.GetConstructor(new Type[] { typeof(int) } ).Invoke(new object[] { Content.Count });

                // Since Content is a List<object>, calling ToArray will give us an object[].
                // Casting to WhateverTheHell[] will cause an exception since it is in fact an object[],
                // nevertheless containing objects of the same class. So we need to artificially (=reflection-wise)
                // create an array of the true type, cast that into an object array and return it.

                for(int i = 0; i < Content.Count; i++)
                    GoodType[i] = Content[i];

                return GoodType;
            }
        }
        
        protected sealed override WidgetType Construct()
        {
            Original.GetFlag<bool>(0, ref AllowsEdit);
            Original.GetFlag<bool>(1, ref AllowsRemove);
            Original.GetFlag<bool>(2, ref AllowsAdd);
            Original.GetFlag<bool>(3, ref GreyButtons);

            Content = new List<object>();
            FieldInfo[] Infos = mUnderlying.GetFields();
            
            List<Type> Primitives = new List<Type>();
            
            foreach(FieldInfo Info in Infos)
            {
                if(IsViewable(Info.FieldType))
                    Primitives.Add(Info.FieldType);
            }
            Primitives.Add(typeof(int));
            
            WidgetType Widget = Construct(Primitives.ToArray());

            foreach(FieldInfo Info in Infos)
            {
                if(IsViewable(Info.FieldType))
                    AddColumn(Info.Name, Info.FieldType);
            }

            Dialog = MakeDialog();

            return Widget;
        }

        #endregion

        #region Abstract members
        protected abstract void AddColumn(string Name, Type Type);
        protected abstract WidgetType Construct(Type[] Types);
        protected abstract void RowAdded(int Id, object[] Items);
        protected abstract void RowEdited(int Id, object[] Items);
        protected abstract bool IsViewable(Type T);
        protected abstract ModalPresenterBase<WidgetType, object> MakeDialog();
        #endregion
    }
}
