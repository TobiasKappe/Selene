using System;
using System.Collections.Generic;
using System.Reflection;

namespace Selene.Backend
{
    public abstract class ListViewerBase<WidgetType> : ConverterBase<WidgetType, object[]>, IHasUnderlying<WidgetType>
    {
        protected int Counter = 0;
        protected ModalPresenterBase<WidgetType> Dialog;
        protected ControlManifest Manifest;
        protected EventHandler OnChange;

        protected bool AllowsEdit = true, AllowsRemove = true,
            AllowsAdd = true, GreyButtons = false;

        int i = 0;
        List<object> Content;
        ConstructorInfo Constructor;
        Type mUnderlying;

        public Type Underlying {
            set
            {
                if((Constructor = value.GetConstructor(Type.EmptyTypes)) == null)
                    throw new InspectionException(value, "Type for list view should contain parameterless constructor");

                mUnderlying = value;
                Manifest = ManifestCache.Retreive(value);
            }
            get { return mUnderlying; }
        }

        #region Manipulation functions
        protected void DeleteRow(int Id)
        {
            Content.RemoveAt(Id);
            i--;
            FireOnChange();
        }

        protected void AddRow()
        {
            object Fill = Constructor.Invoke(null);
            bool Ret = Dialog.Run(mUnderlying, Fill);

            if(!Ret) return;
            Content.Add(Fill);
            RowAdded(i++, BreakItDown(Fill));
            FireOnChange();
        }

        protected void EditRow(int Id)
        {
            Dialog.Run(mUnderlying, Content[Id]);
            RowEdited(Id, BreakItDown(Content[Id]));
            FireOnChange();
        }

        protected void RowChanged(int Id, object[] Values)
        {
            Content[Id] = BreakItBack(Values);
            FireOnChange();
        }

        void FireOnChange()
        {
            if(OnChange != null) OnChange(this, default(EventArgs));
        }
        #endregion

        #region Auxiliary functions
        private object[] BreakItDown(object Begin)
        {
            if(Begin == null) return null;

            List<object> Ret = new List<object>();

            Manifest.EachControl(delegate(ref Control Cont) {
                if(IsViewable(Cont.Type))
                    Ret.Add(Cont.Obtain(Begin));
            });

            return Ret.ToArray();
        }

        private object BreakItBack(object[] Original)
        {
            object Back = Constructor.Invoke(null);

            int i = 0;
            Manifest.EachControl(delegate(ref Control Cont) {
                if(IsViewable(Cont.Type))
                {
                    Cont.Save(Back, Original[i]);
                    i++;
                }
            });

            return Back;
        }
        #endregion

        #region Interface implementation
        protected sealed override object[] ActualValue {
            set
            {
                if(value == null) return;

                Clear();
                i = 0;
                Content.Clear();
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

            List<Type> Primitives = new List<Type>();
            Content = new List<object>();

            Manifest.EachControl(delegate(ref Control Cont) {
                if(IsViewable(Cont.Type))
                    Primitives.Add(Cont.Type);
            });

            Primitives.Add(typeof(int));

            WidgetType Widget = Construct(Primitives.ToArray());

            Manifest.EachControl(delegate(ref Control Cont) {
                if(IsViewable(Cont.Type))
                    AddColumn(Cont.Label, Cont.Type);
            });

            Dialog = MakeDialog();

            return Widget;
        }

        public override event EventHandler Changed {
            add { OnChange += value; }
            remove { OnChange -= value; }
        }

        #endregion

        #region Abstract members
        protected abstract void AddColumn(string Name, Type Type);
        protected abstract WidgetType Construct(Type[] Types);
        protected abstract void RowAdded(int Id, object[] Items);
        protected abstract void RowEdited(int Id, object[] Items);
        protected abstract bool IsViewable(Type T);
        protected abstract ModalPresenterBase<WidgetType> MakeDialog();
        protected abstract void Clear();
        #endregion
    }
}
