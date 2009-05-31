using System;
using System.Collections.Generic;

namespace Selene.Backend
{
    class TempManifest : Dictionary<string, TempCategory>
    {
        public ControlManifest Graduate()
        {
            ControlManifest Ret = new ControlManifest();
            Ret.Categories = new ControlCategory[this.Count];

            int i = 0;
            foreach(string Key in Keys)
            {
                Ret.Categories[i] = this[Key].Graduate(Key);
                i++;
            }

            return Ret;
        }
    }

    class TempCategory : Dictionary<string, TempSubcategory>
    {
        public ControlCategory Graduate(string Name)
        {
            ControlCategory Ret = new ControlCategory();
            Ret.Subcategories = new ControlSubcategory[this.Count];
            Ret.Name = Name;

            int i = 0;
            foreach(string Key in Keys)
            {
                Ret.Subcategories[i] = this[Key].Graduate(Key);
                i++;
            }

            return Ret;
        }
    }

    class TempSubcategory : List<Control>
    {
        public ControlSubcategory Graduate(string Name)
        {
            ControlSubcategory Ret = new ControlSubcategory();
            Ret.Controls = ToArray();
            Ret.Name = Name;

            return Ret;
        }
    }
}
