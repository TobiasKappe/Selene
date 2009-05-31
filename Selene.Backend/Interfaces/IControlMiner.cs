using System;

namespace Selene.Backend
{
	internal interface IControlMiner
	{
		ControlManifest Mine(Type Root);
	}
}
