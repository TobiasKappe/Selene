using System;

namespace Selene.Backend
{
	public interface IValidator<C>
	{
		bool CatIsValid(C Check, int Category);
	}
}
