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

                Control C = new Control(Info, BoundType, ControlFlags, ControlInfo);
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
