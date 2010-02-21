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

using System;
using Selene.Backend;
using Gtk;

namespace Selene.Gtk.Midend
{
    public class StringEntry : ConverterBase<Widget, string>
    {
        protected override string ActualValue {
            get {
                if(Original.SubType == ControlType.Entry)
                    return (Widget as Entry).Text;
                else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                    return (Widget as FileChooserButton).Filename;
                else throw UnsupportedOverride();
            }
            set {
                if(Original.SubType == ControlType.Entry)
                    (Widget as Entry).Text = value ?? string.Empty;
                else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                    (Widget as FileChooserButton).SelectFilename(value ?? string.Empty);
                else throw UnsupportedOverride();
            }
        }
		
        protected override ControlType DefaultSubtype {
            get { return ControlType.Entry; }
        }

        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.DirectorySelect, ControlType.FileSelect };
            }
        }
        protected override Widget Construct ()
        {
            if(Original.SubType == ControlType.Entry)
            {
                return new Entry();
            }
            else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
            {
                string Title = Original.GetFlag<string>();

                return new FileChooserButton(Title ?? "Choose file", Original.SubType == ControlType.FileSelect ?
                                             FileChooserAction.Open : FileChooserAction.SelectFolder);
            }
            else throw UnsupportedOverride();
        }

        void EventChange(EventHandler Subject, bool Add)
        {
            if(Original.SubType == ControlType.Entry)
            {
                if(Add) (Widget as Entry).Changed += Subject;
                else (Widget as Entry).Changed -= Subject;
            }
            else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
            {
                if(Add) (Widget as FileChooserButton).FileSet += Subject;
                else (Widget as FileChooserButton).FileSet -= Subject;
            }
            else throw UnsupportedOverride();
        }

        public override event EventHandler Changed {
            add { EventChange(value, true); }
            remove { EventChange(value, false); }
        }
    }
}
