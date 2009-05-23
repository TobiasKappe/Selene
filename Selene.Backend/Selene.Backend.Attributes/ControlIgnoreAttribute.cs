using System;

namespace Selene.Backend
{	
	[AttributeUsage(AttributeTargets.Field)]
	public class ControlIgnoreAttribute : Attribute
	{
		public ControlIgnoreAttribute()
		{
			
		}
	}
}
