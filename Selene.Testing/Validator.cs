using System;
using System.Text.RegularExpressions;
using Selene.Backend;

#if QYOTO
using Qyoto;
using Selene.Qyoto.Frontend;
#endif

#if GTK
using Gtk;
using Selene.Gtk.Frontend;
#endif

namespace Selene.Testing
{	
	public class ValidTest
	{
		[Control(Category = "Getting started")]
		public string Surname;
		
		[Control(Name = "Phone number")]
		public string PhoneNumber;
		
		[Control(Category = "Finishing up", 
		         Override = ControlType.FileSelect)]
		public string Filename;
		
		[Control(Name = "Do you agree to the license terms?")]
		public bool AgreesLicense;
	}
	
	public enum Page { First, Last }
	
	class Validator : ValidatorBase<ValidTest, Page>
	{
		protected override bool CatIsValid (ValidTest Category, Page Check)
		{
			if(Check == Page.First)
			{
				if(Category.Surname == string.Empty) return false;
				return Regex.IsMatch(Category.PhoneNumber, 
				                     @"^[+][0-9]\d{2}-\d{3}-\d{4}$");
			}
			if(Check == Page.Last)
			{
				if(string.IsNullOrEmpty(Category.Filename)) 
					return false;
				return Category.AgreesLicense;
			}
			
			return true;
		}
	}
	
	public class Validating
	{
		public static void Show()
		{
#if GTK
			var Disp = new WizardDialog<ValidTest>("Selene");
			var Test = new ValidTest();
			Disp.Validator = new Validator();
			Disp.Run(Test);
#endif
		}
	}
}