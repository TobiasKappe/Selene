using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Selene.Backend
{
    public class ControlSubcategory 
    {
        [XmlElement("Control")]
        public Control[] Controls;
        [XmlAttribute]
        public string Name;
        
        public void EachControl(ControlWalker Do)
        {
            for(int i = 0; i < Controls.Length; i++)
            {
                Do(ref Controls[i]);
            }
        }
    }
}
