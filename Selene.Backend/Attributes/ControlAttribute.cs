
using System;

namespace Selene.Backend
{
    public class ControlAttribute : Attribute
    {
        internal string mCategory = null;
        internal string mSubcategory = null;
        internal string mName = null;
        internal ControlType mOverride = ControlType.Default;
        
        public string Category {
            get { return mCategory; }
            set { mCategory = value; }
        }
        
        public string Subcategory {
            get { return mSubcategory; }
            set { mSubcategory = value; }
        }
        
        public string Name {
            get { return mName; }
            set { mName = value; }
        }
        
        public ControlType Override {
            get { return mOverride; }
            set { mOverride = value; }
        }
        
        public ControlAttribute()
        {
        }
        
        public ControlAttribute(string Category)
        {
            this.Category = Category;
        }
        
        public ControlAttribute(string Category, string Subcategory) : this(Category)
        {
            this.Subcategory = Subcategory;
        }
        
        public ControlAttribute(string Category, string Subcategory, string Name) : this(Category, Subcategory)
        {
            this.Name = Name;
        }
    }
}
