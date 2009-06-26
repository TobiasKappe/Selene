using System;
using System.Reflection;

namespace Selene.Backend
{
    public abstract class DisplayBase <SaveType> : IPresenter<SaveType> where SaveType : class
    { 
        protected static ControlManifest Manifest;
        protected static IConverter[] Converters;
        protected static ConstructorInfo ArrayConverter, EnumConverter;
        
        protected SaveType Present;
        protected Control[] State;

        bool HasBuilt = false;
        
        protected static void CacheConverters(Assembly Caller)
        {
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
            Console.WriteLine(Original.Name);
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


        public void Save()
        {
            Save(Present);
        }

        public void Save(SaveType To)
        {
            foreach(Control Cont in State)
            {
                if(Cont.Converter == null) continue;

                object NewValue = Cont.Converter.ToObject(Cont);

                if(Cont.Info.MemberType == MemberTypes.Field)
                    (Cont.Info as FieldInfo).SetValue(To, Cont.Converter.ToObject(Cont));
                else if(Cont.Info.MemberType == MemberTypes.Property)
                    (Cont.Info as PropertyInfo).SetValue(To, Cont.Converter.ToObject(Cont), null);
                else throw new Exception("MemberInfo not field or property. This should not happen");
            }
        }
        #endregion

        public void SubscribeAllChange(EventHandler Handler)
        {
            if(State == null) Build();
            foreach(Control Cont in State)
            {
                if(Cont.Converter != null)
                    Cont.Converter.ConnectChange(Cont, Handler);
            }
        }

        private void SetFields()
        {
            foreach(Control Cont in State)
            {
                object Pass;

                if(Cont.Info.MemberType == MemberTypes.Field)
                    Pass = (Cont.Info as FieldInfo).GetValue(Present);
                else if(Cont.Info.MemberType == MemberTypes.Property)
                    Pass = (Cont.Info as PropertyInfo).GetValue(Present, null);
                else throw new Exception("MemberInfo not field or property. This should not happen");

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
        
        #region Abstract members
        protected abstract void Build();
        public abstract void Hide();
        public abstract void Show();
        #endregion
    }
}
