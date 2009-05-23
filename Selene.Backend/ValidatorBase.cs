using System;

namespace Selene.Backend
{
	public abstract class ValidatorBase<C, T> : IValidator<C> where T : struct where C : class
	{
		private static Array Values;
		
		static ValidatorBase()
		{
			Values = Enum.GetValues(typeof(T));
		}
		
		public ValidatorBase()
		{
			if(!typeof(T).IsEnum) throw new Exception("Validators should have an enum as generic argument");
		}
		
		public bool CatIsValid(C Page, int Category)
		{
			T Cat = (T) Values.GetValue(Category);
			return CatIsValid(Page, Cat);
		}
		
		protected abstract bool CatIsValid(C Category, T Check);
	}
}
