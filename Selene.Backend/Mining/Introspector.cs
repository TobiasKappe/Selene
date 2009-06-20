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
        
        public static IConverter[] GetConverters(Assembly Calling, out ConstructorInfo ArrayConverter, out ConstructorInfo EnumConverter)
        {
            var Ret = new List<IConverter>();
            ArrayConverter = EnumConverter = null;
            
            foreach(Type Browse in Calling.GetTypes())
            {
                if(Browse.ContainsGenericParameters || Browse.IsAbstract) continue;
                
                if(Browse.BaseType == typeof(ListViewerBase))
                {
                    ArrayConverter = Browse.GetConstructor(Type.EmptyTypes);
                    continue;
                }

                if(Browse.BaseType == typeof(EnumBase))
                {
                    EnumConverter = Browse.GetConstructor(Type.EmptyTypes);
                    continue;
                }

                foreach(Type Interface in Browse.GetInterfaces())
                {
                    if(Interface == typeof(IConverter))
                    {
                        ConstructorInfo Info = Browse.GetConstructor(Type.EmptyTypes);
                        if(Info == null)
                            throw new InspectionException(Browse, "Converters should contain an empty constructor, "+Browse+" does not");

                        IConverter Add = (IConverter) Info.Invoke(null);
                        Ret.Add(Add);
                    }
                }
            }
            
            return Ret.ToArray();
        }
    }
}
