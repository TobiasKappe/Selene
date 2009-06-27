using System;
using System.Reflection;
using System.Collections.Generic;

namespace Selene.Backend
{
    public abstract class DisplayBase <WidgetType, SaveType> : IPresenter<SaveType> where SaveType : class
    {
        protected static ControlManifest Manifest;
        protected static ConverterFactory<WidgetType> Factory;

        protected SaveType Present;
        protected List<IConverter<WidgetType>> State;

        bool HasBuilt {
            get { return State.Count > 0; }
        }

        protected static void CacheConverters(Assembly Caller)
        {
            Factory = Introspector.GetConverters<WidgetType>(Caller);

            if(typeof(SaveType) != typeof(object))
                Manifest = Introspector.Inspect(typeof(SaveType));
        }

        internal static void ForceInspect(Type Inspect)
        {
            Manifest = Introspector.Inspect(Inspect);
        }

        public static void StubManifest(string Filename)
        {
            ForceInspect(typeof(SaveType));
            Manifest.Save(Filename);
        }

        protected DisplayBase()
        {
            State = new List<IConverter<WidgetType>>();
        }

        protected IConverter<WidgetType> ProcureState(Control Original)
        {
            if(Original.Type.IsArray && !Original.Type.GetElementType().IsValueType)
            {
                var Viewer = (IHasUnderlying<WidgetType>) Factory.Construct(typeof(Array));

                if(Viewer == null) return null;

                Viewer.Underlying = Original.Type.GetElementType();

                return Viewer;
            }

            if(Original.Type.IsEnum)
            {
                var Viewer = (IHasUnderlying<WidgetType>) Factory.Construct(typeof(Enum));
                Viewer.Underlying = Original.Type;

                return Viewer;
            }

            return Factory.Construct(Original.Type);
        }

        #region Partial interface implementation
        public void Save()
        {
            Save(Present);
        }

        public void Save(SaveType To)
        {
            ConstructState();
            foreach(IConverter<WidgetType> Converter in State)
                Converter.Primitive.Save(To, Converter.Value);
        }
        #endregion

        public void SubscribeAllChange(EventHandler Handler)
        {
            ConstructState();
            foreach(IConverter<WidgetType> Converter in State)
                Converter.Changed += Handler;
        }

        private void SetFields()
        {
            ConstructState();
            foreach(IConverter<WidgetType> Converter in State)
                Converter.Value = Converter.Primitive.Obtain(Present);
        }

        protected void Prepare(SaveType Present)
        {
            this.Present = Present;

            ConstructState();
            SetFields();
        }

        void ConstructState()
        {
            if(HasBuilt) return;
            Build();
        }

        #region Abstract members
        protected abstract void Build();
        public abstract void Hide();
        public abstract void Show();
        #endregion
    }
}
