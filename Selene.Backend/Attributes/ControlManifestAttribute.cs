using System;

namespace Selene.Backend
{   
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class ControlManifestAttribute : Attribute
    {
        string mManifestFile;

        public string ManifestFile {
            get { return mManifestFile; }
        }

        public ControlManifestAttribute(string ManifestFile)
        {
            this.mManifestFile = ManifestFile;
        }
    }
}
