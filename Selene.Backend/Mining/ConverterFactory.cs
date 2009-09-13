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
            if(T == typeof(Enum))
                throw new InspectionException(T, "ConstructEnum should be used for enum types");

            return Make(T);
        }

        public IConverter<WidgetType> ConstructEnum(bool Flags)
        {
            Type T;
            if(Flags) T = typeof(FlagsBase<WidgetType>);
            else T = typeof(EnumBase<WidgetType>);

            return Make(T);
        }

        IConverter<WidgetType> Make(Type T)
        {
            if(!Constructors.ContainsKey(T)) return null;
            else return (IConverter<WidgetType>) Constructors[T].Invoke(null);
        }
    }
}
