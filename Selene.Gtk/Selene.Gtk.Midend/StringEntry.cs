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
            if(Start.SubType == ControlType.FileSelect || Start.SubType == ControlType.DirectorySelect)
                return (Start.Widget as FileChooserButton).Filename;

            return null;
        }

        protected override void SetValue (WidgetPair Original, string Value)
        {
            if(Value == null) Value = string.Empty;

            if(Original.SubType == ControlType.Entry || Original.SubType == ControlType.Default)
                (Original.Widget as Entry).Text = Value;
            if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                (Original.Widget as FileChooserButton).SelectFilename(Value);
        }
                
        protected override WidgetPair ToWidget (WidgetPair Original)
        {
            if(Original.SubType == ControlType.Entry || Original.SubType == ControlType.Default)
            {
                Original.Widget = new Entry();
                return Original;
            }
            if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
            {
                string Title = Original.GetFlag<string>();

                FileChooserButton Ret = new FileChooserButton(Title ?? "Choose file", Original.SubType == ControlType.FileSelect ?
                                                              FileChooserAction.Open : FileChooserAction.SelectFolder);
                                
                Original.Widget = Ret;
                Original.Expands = false;
                return Original;
            }
            
            return null;
        }

        protected override void ConnectChange (WidgetPair Original, System.EventHandler OnChange)
        {
            if(Original.SubType == ControlType.Entry || Original.SubType == ControlType.Default)
                (Original.Widget as Entry).Changed += OnChange;
            else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                (Original.Widget as FileChooserButton).SelectionChanged += OnChange;
        }
    }
}
