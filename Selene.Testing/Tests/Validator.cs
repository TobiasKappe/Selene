using NUnit.Framework;
using Selene.Backend;

using System.Text.RegularExpressions;
using System;
using Gtk;

#if GTK
using Selene.Gtk.Frontend;
#endif

#if QYOTO
using Selene.Qyoto.Frontend;
#endif


namespace Selene.Testing
{
    public class ValidTest : ICloneable
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

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    enum Page { First, Last }

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

    public partial class Harness
    {
        [Test]
        public void Validating()
        {
            var Disp = new WizardDialog<ValidTest>("Selene");
            var Test = new ValidTest();
            Disp.OnDone += delegate {
                Console.WriteLine(Test.AgreesLicense);
                Console.WriteLine(Test.Filename);
                Console.WriteLine(Test.Surname);
                Console.WriteLine(Test.PhoneNumber);
            };
            Disp.Validator = new Validator();
            Disp.Run(Test);
        }
    }
}