using System;
using System.Reflection;
using System.Xml.Serialization;

namespace Selene.Backend
{		
	public class Control
	{		
		public string Label;
		[XmlIgnore] 
		public Type Type;
		[XmlAttribute]
		public ControlType SubType;
		[XmlIgnore] public FieldInfo Info;
		
		[XmlAttribute]
		public string Name;
		
		[XmlElement("Flag")]
		public object[] Flags;
		
		[XmlIgnore]
		public IConverter Converter;
		
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
			Ret.Type = Type;
			Ret.SubType = SubType;
			Ret.Info = Info;
			Ret.Name = Name;
			Ret.Flags = Flags;
			Ret.Converter = Converter;
			
			return Ret;
		}
	}
}
