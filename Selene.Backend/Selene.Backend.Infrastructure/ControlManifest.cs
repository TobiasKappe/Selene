using System;
using System.IO;
using System.Xml.Serialization;

namespace Selene.Backend
{	
	public class ControlManifest
	{		
		[XmlElement("Category")]
		public ControlCategory[] Categories;
		
		public void EachControl(ControlWalker Do)
		{
			foreach(ControlCategory Cat in Categories)
			{
				Cat.EachControl(Do);
			}
		}
		
		public void Save(string Filename)
		{
			XmlSerializer Serializer = new XmlSerializer(typeof(ControlManifest));
			Stream Str = new FileStream(Filename, FileMode.Create);
			Serializer.Serialize(Str, this);
			Str.Flush();
			Str.Close();
		}
	}
}
