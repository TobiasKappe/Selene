
using System;

namespace Selene.Backend
{
	public interface IConverter
	{
		Type Type { get; set; }
		
		Control ToWidget(Control Original, object Value);
		object ToObject(Control Start);
		void SetValue(Control Original, object Value);
	}
}
