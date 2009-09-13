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
using System.Collections.Generic;
using System.Reflection;

namespace Selene.Backend
{
    internal class ReflectionMiner : IControlMiner
    {
        public ControlManifest Mine(Type Root)
        {
            TempManifest Manifest = new TempManifest();
            TempCategory CurrentCat;
            TempSubcategory CurrentSubcat;

            string AddingCat = "Default", AddingSubcat = "Default";
            foreach(MemberInfo Info in Root.GetMembers())
            {
                bool Ignoring = AttributeHelper.GetAttribute<ControlIgnoreAttribute>(Info) != null;
                var ControlInfo = AttributeHelper.GetAttribute<ControlAttribute>(Info);
                var ControlFlags = AttributeHelper.GetAttribute<ControlFlagsAttribute>(Info);

                /* The field might be ignored. However, there could be a category attribute still here,
                   since field attributes are officially given to the next field to be found */
                if(ControlInfo == null)
                {
                    if(AddingCat == null) AddingCat = "Default";
                    if(AddingSubcat == null) AddingSubcat = "Default";
                }
                else
                {
                    if(ControlInfo.Category != null)
                    {
                        AddingCat = ControlInfo.Category;
                        AddingSubcat = "Default";
                    }
                    if(ControlInfo.Subcategory != null) AddingSubcat = ControlInfo.Subcategory;
                }

                if(Ignoring) continue;

                Type BoundType;
                if(Info.MemberType == MemberTypes.Field)
                {
                    FieldInfo Field = Info as FieldInfo;
                    BoundType = Field.FieldType;
                }
                else if(Info.MemberType == MemberTypes.Property)
                {
                    PropertyInfo Property = Info as PropertyInfo;

                    if(!Property.CanRead || !Property.CanWrite) continue;
                    BoundType = Property.PropertyType;
                }
                else continue;

                if(BoundType.IsArray && BoundType.GetElementType() == Root)
                {
                    throw new InspectionException(BoundType, "Types should not contain arrays of themselves, " +
                                                  "to prevent infinite recursion and StackOverflowException. " +
                                                  "Add a ControlIgnore attribute to work around this");
                }

                CurrentCat = Determine<TempCategory>(AddingCat, Manifest);
                CurrentSubcat = Determine<TempSubcategory>(AddingSubcat, CurrentCat);

                Control C = new Control(Info, ControlFlags, ControlInfo);
                CurrentSubcat.Add(C);
            }

            return Manifest.Graduate();
        }

        T Determine<T>(string Key, IDictionary<string, T> Looking) where T : new()
        {
            if(Looking.ContainsKey(Key)) return Looking[Key];
            else
            {
                T Ret;

                Looking.Add(Key, Ret = new T());
                return Ret;
            }
        }
    }
}
