using NUnit.Framework;
using Selene.Backend;

using System;
using Qyoto;
using Gtk;

#if QYOTO
using Selene.Qyoto.Frontend;
#endif

#if GTK
using Selene.Gtk.Frontend;
#endif

namespace Selene.Testing
{
    public partial class Harness
    {
        class GroupingTest
        {
            [Control(Category = "Spare time", Subcategory = "Sports")]
            public string Sport;
            public bool Enjoys;

            [Control(Subcategory = "Preference")]
            public string Music;
            public ushort[] Color;

            [Control(Category = "Employment")]
            public string Employer;
            public DateTime Paycheck;
        }

        [Test]
        public void Grouping()
        {
            GroupingTest Save = new GroupingTest();

            IModalPresenter Present = new NotebookDialog<GroupingTest>("OK if categories fit");
            Assert.IsTrue(Present.Run(Save));

            Present = new ListStoreDialog<GroupingTest>("OK if categories fit");
            Assert.IsTrue(Present.Run(Save));
            
            Present = new TreeStoreDialog<GroupingTest>("OK if categories fit");
            Assert.IsTrue(Present.Run(Save));

#if GTK

            /* Since WizardDialog is not modal, we need to come up with
               another way to signal that the dialog has run. */

            // Present = new WizardDialog<GroupingTest>("OK if categories fit");
            // Assert.IsTrue(Present.Run(Save));
#endif
        }
    }
}
