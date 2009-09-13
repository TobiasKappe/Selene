// Copyright (c) 2009 Tobias Kappé
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// Except as contained in this notice, the name(s) of the above
// copyright holders shall not be used in advertising or otherwise
// to promote the sale, use or other dealings in this Software
// without prior written authorization.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

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
                var Viewer = (IHasUnderlying<WidgetType>)
                    Factory.ConstructEnum(AttributeHelper.GetAttribute<FlagsAttribute>(Original.Type) != null);

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
            SubscribeAllChange(Handler);
        }

        protected void SubscribeAllChange(EventHandler Handler)
        {
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
                throw new Exception("This instance already settled for "+LastType);

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
