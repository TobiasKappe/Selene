using System;
using System.Reflection;
using System.Collections.Generic;

namespace Selene.Backend
{
    public class ConverterFactory<WidgetType>
    {
        public Type For;

        Dictionary<Type, ConstructorInfo> Constructors;

        public ConverterFactory()
        {
            Constructors = new Dictionary<Type, ConstructorInfo>();
        }

        public void AddType(Type Converts, Type Converter)
        {
            ConstructorInfo Info = Converter.GetConstructor(Type.EmptyTypes);
            AddType(Converts, Info);
        }

        public void AddType(Type Converts, ConstructorInfo Converter)
        {
            if(Converter == null || Converter.GetParameters().Length != 0)
                throw new InspectionException(Converter.DeclaringType, "Converters should contain an empty constructor, "
                                              +Converter.DeclaringType+" does not");

            Constructors.Add(Converts, Converter);
        }

        public IConverter<WidgetType> Construct(Type T)
        {
            if(!Constructors.ContainsKey(T)) return null;
            else return (IConverter<WidgetType>) Constructors[T].Invoke(null);
        }
    }
}