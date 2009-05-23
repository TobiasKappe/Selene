using System;
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
	public class Grouping
	{
		public class GroupingTest
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
		
		public static void Show()
		{
			// replace with any of ListStoreDialog, TreeStoreDialog or WizardDialog
			IPresenter<GroupingTest> Present = new NotebookDialog<GroupingTest>("Selene");
			GroupingTest Save = new GroupingTest();
			Present.Run(Save);
			
		}
	}
}
