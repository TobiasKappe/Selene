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

namespace Selene.Backend
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ControlAttribute : Attribute
    {
        string mCategory = null;
        string mSubcategory = null;
        string mName = null;
        ControlType mOverride = ControlType.Default;
        int mWidth, mHeight;
        
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
        
        public int Width {
            get { return mWidth; }
            set { mWidth = value; }
        }
        
        public int Height {
            get { return mHeight; }
            set { mHeight = value; }
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
