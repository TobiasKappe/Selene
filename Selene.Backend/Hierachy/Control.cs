using System;
using System.Reflection;
using System.Xml.Serialization;

namespace Selene.Backend
{
    public class Control
    {
        static Exception MemberInfoWrong;

        static Control()
        {
            MemberInfoWrong = new Exception("MemberInfo not field or property. This should not happen");
        }

        public string Label;

        [XmlIgnore]
        public MemberInfo Info;

        [XmlIgnore]
        public Type Type {
            get
            {
                if(Info.MemberType == MemberTypes.Field)
                    return (Info as FieldInfo).FieldType;
                else if(Info.MemberType == MemberTypes.Property)
                    return (Info as PropertyInfo).PropertyType;
                else throw MemberInfoWrong;
            }
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

        public Control(MemberInfo Info, ControlFlagsAttribute FlagsAttr, ControlAttribute Attribute)
        {
            this.Info = Info;
            Flags = FlagsAttr == null ? null : FlagsAttr.Flags;

            // Use field name if no label is specified
            Label = Attribute == null || Attribute.Name == null ? Info.Name : Attribute.Name;
            SubType = Attribute == null ? ControlType.Default : Attribute.Override;
        }

        public void Save(object To, object Value)
        {
            if(Info.MemberType == MemberTypes.Field)
                (Info as FieldInfo).SetValue(To, Value);
            else if(Info.MemberType == MemberTypes.Property)
                (Info as PropertyInfo).SetValue(To, Value, null);
            else throw MemberInfoWrong;
        }

        public object Obtain(object From)
        {
            if(Info.MemberType == MemberTypes.Field)
                return (Info as FieldInfo).GetValue(From);
            else if(Info.MemberType == MemberTypes.Property)
                return (Info as PropertyInfo).GetValue(From, null);
            else throw MemberInfoWrong;
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
    }
}
