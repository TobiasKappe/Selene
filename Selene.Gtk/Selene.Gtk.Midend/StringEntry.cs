using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
	public class StringEntry : ConverterBase<WidgetPair, string>
	{		
		public StringEntry() : base(ControlType.DirectorySelect, ControlType.FileSelect, ControlType.Entry)
		{
		}
		
		protected override string ToValue (WidgetPair Start)
		{
			if(Start.SubType == ControlType.Entry || Start.SubType == ControlType.Default)
				return (Start.Widget as Entry).Text;
			else if(Start.SubType == ControlType.FileSelect)
				return (Start.Widget as FileChooserButton).Filename;
			
			return null;
		}
		
		protected override void SetValue (WidgetPair Original, string Value)
		{
			if(Value == null) Value = string.Empty;
			
			if(Original.Widget is Entry)
				(Original.Widget as Entry).Text = Value;
			if(Original.Widget is FileChooserButton)
				(Original.Widget as FileChooserButton).SelectFilename(Value);
		}
				
		protected override WidgetPair ToWidget (WidgetPair Original, string Value)
		{
			if(Original.SubType == ControlType.Entry || Original.SubType == ControlType.Default)
			{
				Original.Widget = new Entry(Value ?? string.Empty);
				return Original;
			}
			if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
			{
				string Title = Original.GetFlag<string>();
				
				FileChooserButton Ret = new FileChooserButton(Title ?? "Choose file", Original.SubType == ControlType.FileSelect ? 
				                							  FileChooserAction.Open : FileChooserAction.SelectFolder);
				if(Value != null) 
				{
					if(!Ret.SelectFilename(Value)) Ret.WidthChars = 8;
					else Ret.WidthChars = System.IO.Path.GetFileName(Value).Length;
				}
				else Ret.WidthChars = 8;
								
				Original.Widget = Ret;
				Original.Expands = false;
				return Original;
			}
			
			return null;
		}
	}
}
