using System;
using System.Collections.Generic;
using System.Reflection;

namespace Selene.Backend
{
    public abstract class ListViewerBase : IHasUnderlying
    {
        protected int Counter = 0;
        protected ModalPresenterBase<object> Presenter;
        protected bool AllowsEdit = true, AllowsRemove = true, AllowsAdd = true, GreyButtons = false;

        int i = 0;
        List<object> Content;
        ConstructorInfo Constructor;
        bool Inspected = false;
        Type Underlying;

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
                Presenter.ForceInspect(Fill.GetType());
                Inspected = true;          
            }
            bool Ret = Presenter.Run(Fill);
            
            if(!Ret) return;
            Content.Add(Fill);
            RowAdded(i++, BreakItDown(Fill));
        }
        
        protected void EditRow(int Id)
        {
            Presenter.Run(Content[Id]);
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
        public Type Type 
        { 
            get { return null; }
        }

        public void SetUnderlying(Type Given)
        {
            if((Constructor = Given.GetConstructor(Type.EmptyTypes)) == null)
                throw new InspectionException(Given, "Type for list view should contain paramaterless constructor");

            Underlying = Given;
        }

        public void SetValue(Control asd, object Value)
        {
            foreach(object Item in (object[])Value)
            {
                object[] Columns = BreakItDown(Item);
                Content.Add(Item);
                RowAdded(i++, Columns);
            }
        }
        
        public object ToObject(Control Start)
        {
            Type ToArray = Underlying.MakeArrayType();
            
            // And now for the ugliest line in C# history:
            object[] GoodType = (object[]) ToArray.GetConstructor(new Type[] { typeof(int) } ).Invoke(new object[] { Content.Count });
            
            // Explanation of the above: since Content is a List<object>, calling ToArray will give us an object[].
            // Casting to WhateverTheHell[] will cause an exception since it is in fact an object[], 
            // nevertheless containing objects of the same class. So we need to artificially (=reflection-wise) 
            // create an array of the true type, cast that into an object array and return it.
            
            for(int i = 0; i < Content.Count; i++) 
                GoodType[i] = Content[i];
            
            return GoodType;
        }
        
        public Control ToWidget(Control Original)
        {
            Original.GetFlag<bool>(0, ref AllowsEdit);
            Original.GetFlag<bool>(1, ref AllowsRemove);
            Original.GetFlag<bool>(2, ref AllowsAdd);
            Original.GetFlag<bool>(3, ref GreyButtons);

            Content = new List<object>();
            FieldInfo[] Infos = Underlying.GetFields();         
            
            List<Type> Primitives = new List<Type>();
            
            foreach(FieldInfo Info in Infos)
            {
                if(IsViewable(Info.FieldType))
                    Primitives.Add(Info.FieldType);
            }
            Primitives.Add(typeof(int));
            
            Presenter = Construct(Primitives.ToArray(), ref Original);
            
            foreach(FieldInfo Info in Infos)
            {
                if(IsViewable(Info.FieldType))
                    AddColumn(Info.Name, Info.FieldType);
            }

            return Original;
        }

        public void ConnectChange(Control Original, EventHandler OnChange)
        {
            ConnectChange(OnChange);
        }

        #endregion

        #region Abstract members
        protected abstract void AddColumn(string Name, Type Type);
        protected abstract ModalPresenterBase<object> Construct(Type[] Types, ref Control Cont);
        protected abstract void RowAdded(int Id, object[] Items);
        protected abstract void RowEdited(int Id, object[] Items);
        protected abstract bool IsViewable(Type T);
        protected abstract void ConnectChange(EventHandler OnChange);
        #endregion
    }
}
