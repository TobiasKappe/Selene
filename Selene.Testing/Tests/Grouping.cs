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
        class GroupingTest : ICloneable
        {
            #pragma warning disable 0649
            
            [Control(Category = "Spare time", Subcategory = "Sports")]
            public string Sport;
            public bool Enjoys;

            [Control(Subcategory = "Preference")]
            public string Music;
            public ushort[] Color;

            [Control(Category = "Employment")]
            public string Employer;
            public DateTime Paycheck;

            public object Clone ()
            {
                return MemberwiseClone();
            }
        }

        const string Title = "OK if categories fit";

        [Test]
        public void Grouping()
        {
            GroupingTest Save = new GroupingTest();

            IModalPresenter Present = new NotebookDialog<GroupingTest>(Title);
            Assert.IsTrue(Present.Run(Save));

            Present = new ListStoreDialog<GroupingTest>(Title);
            Assert.IsTrue(Present.Run(Save));

            Present = new TreeStoreDialog<GroupingTest>(Title);
            Assert.IsTrue(Present.Run(Save));

            var NonModal = new WizardDialog<GroupingTest>(Title);
            NonModal.Run(Save);
            NonModal.Block();
            Assert.IsTrue(NonModal.Success);
        }
    }
}
