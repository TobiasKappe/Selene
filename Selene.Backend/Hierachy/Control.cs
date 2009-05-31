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
		
		public T GetFlag<T>() where T : class
		{
			if(Flags == null) return null;
			
			foreach(object Flag in Flags)
			{
				if(Flag is T) return Flag as T;
			}
			
			return null;
		}
		
		public bool GetFlag<T>(ref T Ret) where T : struct
		{
			if(Flags == null) return false;
			
			foreach(object Flag in Flags)
			{
				T? Try = Flag as T?;
				if(Try != null) 	
				{
					Ret = Try.HasValue ? Try.Value : default(T);
					return Try.HasValue;
				}
			}
			
			return false;
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
