
using System;

namespace Selene.Backend
{
	public enum WarningMethod { ThrowException, LogToConsole }
	
	public static class WarningFactory
	{
		public static WarningMethod Method = WarningMethod.LogToConsole;
		
		public static void Warn(string Warning)
		{
			if(Method == WarningMethod.ThrowException) throw new Exception(Warning);
			if(Method == WarningMethod.LogToConsole)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("[Warning] ");
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine(Warning);
			}
		}
	}
}
