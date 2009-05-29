
using System;

namespace Selene.Backend
{	
	public interface IPresenter<T>
	{
		void Save();
		bool Run(T Present);
		
		void Show();
		void Hide();
	}
}
