using System;

namespace Selene.Backend
{	
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class ControlManifestAttribute : Attribute
	{
		public string ManifestFile;
		
		public ControlManifestAttribute(string ManifestFile)
		{
			this.ManifestFile = ManifestFile;
		}
	}
}
