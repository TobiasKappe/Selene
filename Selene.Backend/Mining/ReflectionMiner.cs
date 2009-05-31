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
			foreach(FieldInfo Info in Root.GetFields())
			{
				var Ignore = AttributeHelper.GetAttribute<ControlIgnoreAttribute>(Info);
				var ControlInfo = AttributeHelper.GetAttribute<ControlAttribute>(Info);
				var ControlFlags = AttributeHelper.GetAttribute<ControlFlagsAttribute>(Info);

				if(Ignore == null && Info.FieldType.IsArray && Info.FieldType.GetElementType() == Root)
				{
					throw new InspectionException(Info.FieldType, "Types should not contain arrays of themselves, " +
					                              "to prevent infinite recursion and StackOverflowException. " +
					                              "Add a ControlIgnore attribute to work around this");
				}

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

				if(Ignore != null) continue;

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
