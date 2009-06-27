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

                if(Browse.BaseType == typeof(EnumBase<WidgetType>))
                {
                    Factory.AddType(typeof(Enum), Browse);
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
