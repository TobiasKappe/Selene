// Copyright (c) 2009 Tobias Kappé
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// Except as contained in this notice, the name(s) of the above
// copyright holders shall not be used in advertising or otherwise
// to promote the sale, use or other dealings in this Software
// without prior written authorization.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using NUnit.Framework;
using Selene.Backend;

using System.Text.RegularExpressions;
using System;

#if GTK
using Selene.Gtk.Frontend;
#endif

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

#if WINDOWS
using Selene.Winforms.Frontend;
#endif


namespace Selene.Testing
{
    /* Validator test - checks the validation possiblility provided by the
     * backend. The surname should be non-empty, the phone-number should
     * conform to the regex found below. If these two conditions apply, 
     * the "next" button should become clickable. Next, the file should be
     * chosen and the checkbox checked, after which the "complete" button
     * should be enabled. Test fails if window is closed, succeeds if user
     * reaches the "complete" button. */
    
    public class ValidTest : ICloneable
    {
        #pragma warning disable 0649
        
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
            
            Disp.Validator = new Validator();
            Disp.Run(Test);
            Disp.Block();
            Assert.IsTrue(Disp.Success);
        }
    }
}