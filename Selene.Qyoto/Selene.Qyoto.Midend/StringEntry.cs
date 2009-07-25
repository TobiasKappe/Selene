using System;
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
                if(Orig.SubType == ControlType.Entry || Orig.SubType == ControlType.Default)
                    return (Widget as QLineEdit).Text;
                else return Selected;
            }
            set
            {
                if(Orig.SubType == ControlType.Entry || Orig.SubType == ControlType.Default)
                    (Widget as QLineEdit).Text = value;
                else if(Orig.SubType == ControlType.FileSelect || Orig.SubType == ControlType.DirectorySelect)
                {
                    if(value != null && value != "")
                    {
                        Selected = value;
                        (Widget as QPushButton).Text = System.IO.Path.GetFileName(Selected);
                    }
                }
            }
        }

        protected override ControlType[] Supported {
            get
            {
                return new ControlType[] { ControlType.Entry, ControlType.FileSelect, ControlType.DirectorySelect };
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
            else return new QLineEdit();
        }

        protected override string SignalForType (ControlType Type)
        {
            if(Orig.SubType == ControlType.Entry || Orig.SubType == ControlType.Default)
                return "textChanged(QString)";
            else return null;
        }

        void ButtonClicked()
        {
            QFileDialog Dialog = new QFileDialog();
            if(Orig.SubType == ControlType.DirectorySelect)
                Dialog.fileMode = QFileDialog.FileMode.DirectoryOnly;
            else if(Orig.SubType == ControlType.FileSelect)
                Dialog.fileMode = QFileDialog.FileMode.ExistingFile;

            if(Dialog.Exec() != 0)
            {
                Selected = Dialog.SelectedFiles()[0];
                (Widget as QPushButton).Text = System.IO.Path.GetFileName(Selected);
                FireChanged();
            }
        }
    }
}
