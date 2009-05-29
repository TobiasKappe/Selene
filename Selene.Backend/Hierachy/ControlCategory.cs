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
		[XmlAttribute("Count")] 
		public int ControlCount = 0;
		
		public void EachControl(ControlWalker Do)
		{
			foreach(ControlSubcategory Subcat in Subcategories)
			{
				Subcat.EachCategory(Do);
			}
		}
	}
}
