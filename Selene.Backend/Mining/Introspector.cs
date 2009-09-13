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
using System.Xml.Serialization;
using System.Resources;
using System.IO;

namespace Selene.Backend
{
    public enum ControlType { Default, Entry, Radio, Dropdown, Check, Toggle, FileSelect, DirectorySelect, Color }
    
    public delegate void ControlWalker(ref Control Current);
    
    internal static class Introspector
    {
        public static ControlManifest Inspect(Type Root)
        {
            var Manifest = AttributeHelper.GetAttribute<ControlManifestAttribute>(Root);

            IControlMiner Miner;
            if(Manifest != null) Miner = new XmlMiner(Manifest.ManifestFile);
            else Miner = new ReflectionMiner();

            return Miner.Mine(Root);
        }

        public static ConverterFactory<WidgetType> GetConverters<WidgetType>(Assembly Calling)
        {
            var Factory = new ConverterFactory<WidgetType>();

            foreach(Type Browse in Calling.GetTypes())
            {
                if(Browse.ContainsGenericParameters || Browse.IsAbstract) continue;

                if(Browse.BaseType == typeof(ListViewerBase<WidgetType>))
                {
                    Factory.AddType(typeof(Array), Browse);
                    continue;
                }

                if(Browse.BaseType == typeof(EnumBase<WidgetType>) || Browse.BaseType == typeof(FlagsBase<WidgetType>))
                {
                    Factory.AddType(Browse.BaseType, Browse);
                    continue;
                }

                foreach(Type Interface in Browse.GetInterfaces())
                {
                    if(Interface == typeof(IConverter<WidgetType>))
                    {
                        ConstructorInfo Info = Browse.GetConstructor(Type.EmptyTypes);
                        if(Info == null)
                            throw new InspectionException(Browse, "Converters should contain an empty constructor, "+Browse+" does not");

                        // We need to instantiate to discover the type... may need to find a better way
                        IConverter<WidgetType> Add = (IConverter<WidgetType>) Info.Invoke(null);

                        Factory.AddType(Add.Type, Info);
                    }
                }
            }

            return Factory;
        }
    }
}
