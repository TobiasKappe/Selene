using System;
using System.Collections.Generic;
using System.Reflection;

namespace Selene.Backend
{
    public delegate void Done();
    
    public abstract class DisplayBase <SaveType> : IPresenter<SaveType> where SaveType : class
    { 
        protected static ControlManifest Manifest;
        protected static IConverter[] Converters;
        protected static ConstructorInfo ArrayConverter, EnumConverter;
        
        protected SaveType Present;
        protected Control[] State;
        
        public event Done OnDone;
        bool HasBuilt = false;
        
        static DisplayBase()
        {
            Assembly Caller = Assembly.GetCallingAssembly();
            Converters = Introspector.GetConverters(Caller, out ArrayConverter, out EnumConverter);
            
            if(Converters.Length < 1) WarningFactory.Warn("No converters found in assembly "+Caller.FullName+". Presenters will be empty");
            
            if(typeof(SaveType) != typeof(object))
                Manifest = Introspector.Inspect(typeof(SaveType));
        }
        
        internal void ForceInspect(Type Inspect)
        {
            Manifest = Introspector.Inspect(Inspect);
        }
        
        public static void StubManifest(string Filename)
        {
            Manifest.Save(Filename);
        }
        
        protected DisplayBase()
        {
        }
        
        protected Control ProcureState(Control Original)
        {
            if(Original.Type.IsArray && ArrayConverter != null && !Original.Type.GetElementType().IsValueType)
            {
                IHasUnderlying Viewer = (IHasUnderlying)ArrayConverter.Invoke(null);
                Viewer.SetUnderlying(Original.Type.GetElementType());
                Original = Viewer.ToWidget(Original);
                Original.Converter = Viewer;

                return Original;
            }

            if(Original.Type.IsEnum)
            {
                IHasUnderlying Viewer = (IHasUnderlying)EnumConverter.Invoke(null);
                Viewer.SetUnderlying(Original.Type);
                Original = Viewer.ToWidget(Original);
                Original.Converter = Viewer;

                return Original;
            }

            foreach(IConverter Converter in Converters)
            {
                if(Converter.Type == Original.Type)
                {
                    Control Add = Converter.ToWidget(Original);
                    Add.Converter = Converter;
                    return Add;
                }
            }

            return null;
        }
        
        #region Partial interface implementation
        public virtual bool Run(SaveType Present)
        {
            Prepare(Present);
            Show();

            return true;
        }

        public void Save()
        {
            Save(Present);
        }

        public void Save(SaveType To)
        {
            foreach(Control Cont in State)
            {
                if(Cont.Converter == null) continue;
                Cont.Info.SetValue(To, Cont.Converter.ToObject(Cont));
            }
        }
        #endregion

        private void SetFields()
        {
            foreach(Control Cont in State)
            {
                object Pass = Cont.Info.GetValue(Present);
                Cont.Converter.SetValue(Cont, Pass);
            }
        }
        
        protected void Prepare(SaveType Present)
        {
            this.Present = Present; 

            if(!HasBuilt)
            {
                Build();
                HasBuilt = true;
            }
            SetFields();
        }
        
        protected void FireDone()
        {
            if(OnDone != null) OnDone();
        }
        
        #region Abstract members
        protected abstract void Build();
        public abstract void Hide();
        public abstract void Show();
        #endregion
    }
}
