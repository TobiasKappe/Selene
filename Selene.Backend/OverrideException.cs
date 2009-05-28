using System;
using System.Text;

namespace Selene.Backend
{
	public class OverrideException : Exception
	{
		public OverrideException(Type Target, ControlType Fault, params ControlType[] Supported) 
			: base(BuildMessage(Target, Fault, Supported))
		{

		}
		
		private static string BuildMessage(Type Target, ControlType Fault, ControlType[] Supported)
		{
			StringBuilder Alternatives = new StringBuilder();
			Alternatives.Append(string.Format("Override {0} not supported for type {1}, try using ", Fault, Target));
			
			for(int i = 0; i < Supported.Length; i++)
			{
				Alternatives.Append(Supported[i]);
				if(i == Supported.Length-2) Alternatives.Append(" or ");
				else if(i < Supported.Length-1) Alternatives.Append(", ");
			}
			
			return Alternatives.ToString();
		}
	}
}
