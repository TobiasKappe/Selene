
using System;

namespace Selene.Backend
{	
	public interface IEmbeddable<P, T>
	{
		T Content(P Present);
		bool IsEmbedded { get; }
	}
}
