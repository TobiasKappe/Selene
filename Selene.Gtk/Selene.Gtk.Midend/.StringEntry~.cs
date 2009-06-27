using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class StringEntry : ConverterBase<Widget, string>
    {
        protected override string ActualValue {
            get {
                if(Original.SubType == ControlType.Entry || Original.SubType == ControlType.Default)
                    return (Widget as Entry).Text;
                if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                    return (Widget as FileChooserButton).Filename;

                return null;
            }
            set {
                if(Original.SubType == ControlType.Entry || Original.SubType == ControlType.Default)
                    (Widget as Entry).Text = value ?? string.Empty;
                if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                    (Widget as FileChooserButton).SelectFilename(value ?? string.Empty);
            }
        }

        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.DirectorySelect, ControlType.FileSelect, ControlType.Entry };
            }
        }
        protected override Widget Construct ()
        {
            if(Original.SubType == ControlType.Entry || Original.SubType == ControlType.Default)
            {
                return new Entry();
            }
            if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
            {
                string Title = Original.GetFlag<string>();

                return new FileChooserButton(Title ?? "Choose file", Original.SubType == ControlType.FileSelect ?
                                             FileChooserAction.Open : FileChooserAction.SelectFolder);
            }

            return null;
        }

        void EventChange(EventHandler Subject, bool Add)
        {
            if(Original.SubType == ControlType.Entry || Original.SubType == ControlType.Default)
            {
                if(Add) (Widget as Entry).Changed += Subject;
                else (Widget as Entry).Changed -= Subject;
            }
            else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
            {
                if(Add) (Widget as FileChooserButton).SelectionChanged += Subject;
                else (Widget as FileChooserButton).SelectionChanged -= Subject;
            }
        }

        public override event EventHandler Changed {
            add { EventChange(value, true); }
            remove { EventChange(value, false); }
        }
    }
}
