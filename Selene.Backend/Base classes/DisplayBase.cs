using System;
using System.Reflection;
using System.Collections.Generic;

namespace Selene.Backend
{
    public abstract class DisplayBase <WidgetType> : IPresenter
    {
        protected static ConverterFactory<WidgetType> Factory;

        protected List<IConverter<WidgetType>> State;
        protected Type LastType;
        protected object Present;

        bool HasBuilt {
            get { return State.Count > 0; }
        }

        internal static void CacheConverters(Assembly Calling)
        {
            Factory = Introspector.GetConverters<WidgetType>(Calling);
        }

        public static void StubManifest<T>(string Filename)
        {
            ManifestCache.Retreive(typeof(T)).Save(Filename);
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

        public void Save(object To)
        {
            foreach(IConverter<WidgetType> Converter in State)
                Converter.Primitive.Save(To, Converter.Value);
        }
        #endregion

        public void SubscribeAllChange<T>(EventHandler Handler)
        {
            Prepare(typeof(T), null, false);
            foreach(IConverter<WidgetType> Converter in State)
                Converter.Changed += Handler;
        }

        internal void SetFields()
        {
            foreach(IConverter<WidgetType> Converter in State)
                Converter.Value = Converter.Primitive.Obtain(Present);
        }

        protected void Prepare(Type For, object Present, bool DoShow)
        {
            if(LastType == null)
            {
                LastType = For;
                State = new List<IConverter<WidgetType>>();
                Build(ManifestCache.Retreive(For));
            }
            else if(For != LastType)
            {
                throw new Exception("This type already settled for "+LastType);
            }

            this.Present = Present;

            if(Present != null) SetFields();

            if(DoShow) Show();
        }

        protected void Prepare(Type For, object Present)
        {
            Prepare(For, Present, true);
        }

        #region Abstract members
        protected abstract void Build(ControlManifest Manifest);

        public abstract void Hide();
        public abstract void Show();
        #endregion
    }
}
