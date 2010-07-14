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

using Selene.Backend;
using Qyoto;

namespace Selene.Qyoto.Midend
{
    public class StringEntry : QConverterProxy<string>
    {
        string Selected = "";

        protected override string ActualValue {
            get
            {
                if(Orig.SubType == ControlType.Entry)
                    return (Widget as QLineEdit).Text;
                else if(Orig.SubType == ControlType.FileSelect || Orig.SubType == ControlType.DirectorySelect)
                    return Selected;
                else throw UnsupportedOverride();
            }
            set
            {
                if(Orig.SubType == ControlType.Entry)
                    (Widget as QLineEdit).Text = value;
                else if(Orig.SubType == ControlType.FileSelect || Orig.SubType == ControlType.DirectorySelect)
                {
                    if(value != null && value != "")
                    {
                        Selected = value;
                        (Widget as QPushButton).Text = System.IO.Path.GetFileName(Selected);
                    }
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

        protected override QObject Construct ()
        {
            if(Orig.SubType == ControlType.FileSelect || Orig.SubType == ControlType.DirectorySelect)
            {
                QPushButton Button = new QPushButton();

                if(Orig.SubType == ControlType.FileSelect)
                    Button.Text = "Select file";
                else Button.Text = "Select directory";

                QWidget.Connect(Button, Qt.SIGNAL("clicked()"), ButtonClicked);
                // TODO: Get an icon in here as soon as Qt has decent stock handling
                return Button;
            }
            else if(Orig.SubType == ControlType.Entry)
                return new QLineEdit();
            else throw UnsupportedOverride();
        }

        protected override string SignalForType (ControlType Type)
        {
            if(Orig.SubType == ControlType.Entry)
                return "textChanged(QString)";
            else if(Orig.SubType == ControlType.FileSelect || Orig.SubType == ControlType.DirectorySelect)
                return null;
            else throw UnsupportedOverride();
        }

        void ButtonClicked()
        {
            QFileDialog Dialog = new QFileDialog();
            if(Orig.SubType == ControlType.DirectorySelect)
                Dialog.fileMode = QFileDialog.FileMode.DirectoryOnly;
            else if(Orig.SubType == ControlType.FileSelect)
                Dialog.fileMode = QFileDialog.FileMode.ExistingFile;
            
            Dialog.SetWindowTitle(Original.GetFlag<string>());

            if(Dialog.Exec() != 0)
            {
                Selected = Dialog.SelectedFiles()[0];
                (Widget as QPushButton).Text = System.IO.Path.GetFileName(Selected);
                FireChanged();
            }
        }
    }
}
