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
using Forms = System.Windows.Forms;
using System.Windows.Forms;

namespace Selene.Winforms.Midend
{
    public class StringEntry : ConverterBase<Forms.Control, string>
    {
        string File;
        EventHandler Proxy;

        protected override string ActualValue {
            get
            {
                if(Original.SubType == ControlType.Entry)
                    return (Widget as TextBox).Text;
                else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                    return File;
                else throw UnsupportedOverride();
            }
            set
            {
                if(Original.SubType == ControlType.Entry)
                    (Widget as TextBox).Text = value;
                else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                {
                    File = value;
                    if(File != string.Empty && File != null)
                        (Widget as Button).Text = System.IO.Path.GetFileName(value);
                }
                else throw UnsupportedOverride();
            }
        }
		
        protected override ControlType DefaultSubtype {
            get { return ControlType.Entry; }
        }

        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.FileSelect, ControlType.DirectorySelect };
            }
        }

        protected override Forms.Control Construct ()
        {
            if(Original.SubType == ControlType.Entry)
                return new TextBox();
            else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
            {
                Button Ret = new Button();
                Ret.Text = "Choose a " + (Original.SubType == ControlType.DirectorySelect ? "directory" : "file");
                Ret.Click += ButtonClick;
                return Ret;
            }
            else throw UnsupportedOverride();
        }

        void ButtonClick (object sender, EventArgs e)
        {
            if(Original.SubType == ControlType.FileSelect)
            {
                FileDialog Show = new OpenFileDialog();
                Show.CheckFileExists = true;
                Show.CheckPathExists = true;
                Show.FileName = File;
                if(Show.ShowDialog() == DialogResult.OK)
                {
                    ActualValue = Show.FileName;
                    if(Proxy != null) Proxy(sender, null);
                }
            }
            else if(Original.SubType == ControlType.DirectorySelect)
            {
                FolderBrowserDialog Show = new FolderBrowserDialog();
                Show.SelectedPath = File;
                if(Show.ShowDialog() == DialogResult.OK)
                {
                    ActualValue = Show.SelectedPath;
                    if(Proxy != null) Proxy(sender, null);
                }
            }
            else if(Original.SubType != ControlType.Entry)
                throw UnsupportedOverride();
        }

        public override event EventHandler Changed {
            add
            {
                if(Original.SubType == ControlType.Entry)
                    (Widget as TextBox).TextChanged += value;
                else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                    Proxy += value;
                else throw UnsupportedOverride();
            }
            remove
            {
                if(Original.SubType == ControlType.Entry)
                    (Widget as TextBox).TextChanged -= value;
                else if(Original.SubType == ControlType.FileSelect || Original.SubType == ControlType.DirectorySelect)
                    Proxy -= value;
                else throw UnsupportedOverride();
            }
        }
    }
}
