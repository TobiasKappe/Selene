
using System;

namespace Selene.Backend
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ControlAttribute : Attribute
    {
        string mCategory = null;
        string mSubcategory = null;
        string mName = null;
        ControlType mOverride = ControlType.Default;
        
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
