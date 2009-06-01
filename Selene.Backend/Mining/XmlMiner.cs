using System;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace Selene.Backend
{
    internal class XmlMiner : IControlMiner
    {
        string ManifestFile;
        Type Root;
        
        public XmlMiner(string Read)
        {
            ManifestFile = Read;
        }
        
        public ControlManifest Mine(Type Root)
        {
            this.Root = Root;
            XmlSerializer Serializer = new XmlSerializer(typeof(ControlManifest));
            
            ControlManifest Ret = null;

            using (FileStream Stream = new FileStream(ManifestFile, FileMode.Open))
            {
                Ret = Serializer.Deserialize(Stream) as ControlManifest;
            }

            Ret.EachControl(FillControl);
            
            return Ret;
        }
            
        void FillControl(ref Control Fill)
        {
            FieldInfo Info = Root.GetField(Fill.WantedName);

            if(Info == null)
                throw new InspectionException(Root, "Type "+Root+" does not contain field "+Fill.WantedName);
            
            Fill.Info = Info;
        }
    }
}
