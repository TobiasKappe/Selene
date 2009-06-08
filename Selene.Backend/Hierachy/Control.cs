using System;
using System.Reflection;
using System.Xml.Serialization;

namespace Selene.Backend
{
    public class Control
    {
        public string Label;

        [XmlIgnore] public IConverter Converter;
        [XmlIgnore] public FieldInfo Info;

        [XmlIgnore]
        public Type Type {
            get { return Info.FieldType; }
        }

        // Loaded by XmlMiner
        public string WantedName;

        [XmlAttribute]
        public string Name {
            get { return Info.Name; }
            set { WantedName = value; }
        }

        [XmlAttribute]
        public ControlType SubType;

        [XmlElement("Flag")]
        public object[] Flags;

        public Control()
        {
        }

        public Control(FieldInfo Info, ControlFlagsAttribute FlagsAttr, ControlAttribute Attribute)
        {
            this.Info = Info;
            if(Info == null) Console.WriteLine("WARNING null info");
            Flags = FlagsAttr == null ? null : FlagsAttr.Flags;

            // Use field name if no label is specified
            Label = Attribute == null || Attribute.Name == null ? Info.Name : Attribute.Name;
            SubType = Attribute == null ? ControlType.Default : Attribute.Override;
        }

        public T GetFlag<T>(int Num) where T : class
        {
            if(Flags == null) return null;

            int Current = 0;
            foreach(object Flag in Flags)
            {
                if(Flag is T)
                {
                    if(Current == Num) return Flag as T;
                    Current++;
                }
            }

            return null;
        }

        public T GetFlag<T>() where T : class
        {
            return GetFlag<T>(0);
        }

        public bool GetFlag<T>(int Num, ref T Ret) where T : struct
        {
            if(Flags == null) return false;

            int Current = 0;
            foreach(object Flag in Flags)
            {
                T? Try = Flag as T?;
                if(Try != null)
                {
                    if(Current == Num)
                    {
                        Ret = Try.HasValue ? Try.Value : default(T);
                        return Try.HasValue;
                    }
                    Current++;
                }
            }

            return false;
        }

        public bool GetFlag<T>(ref T Ret) where T : struct
        {
            return GetFlag<T>(0, ref Ret);
        }

        public T Subclassify<T>() where T : Control, new()
        {
            T Ret = new T();
            Ret.Label = Label;
            Ret.SubType = SubType;
            Ret.Info = Info;
            Ret.Flags = Flags;
            Ret.Converter = Converter;
            
            return Ret;
        }
    }
}
