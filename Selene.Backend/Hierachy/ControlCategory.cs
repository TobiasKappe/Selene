using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Selene.Backend
{
	public class ControlCategory
	{
		[XmlElement("Subcategory")]
		public ControlSubcategory[] Subcategories;
		[XmlAttribute]
		public string Name;

		[XmlIgnore]
		public int ControlCount
        {
            get {
                int Ret = 0;
                foreach(ControlSubcategory Subcat in Subcategories)
                {
                    Ret += Subcat.Controls.Length;
                }
                return Ret;
            }
        }
		
		public void EachControl(ControlWalker Do)
		{
			foreach(ControlSubcategory Subcat in Subcategories)
			{
				Subcat.EachControl(Do);
			}
		}
	}
}
