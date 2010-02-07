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
    public class BoolToggler : QConverterProxy<bool>
    {
        protected override bool ActualValue {
            get
            {
                if(Original.SubType == ControlType.Check)
                    return (Widget as QCheckBox).Checked;
                else if(Original.SubType == ControlType.Toggle)
                    return (Widget as QPushButton).Checked;
                
                return false;
            }
            set
            {
                if(Original.SubType == ControlType.Check)
                    (Widget as QCheckBox).Checked = value;
                else if(Original.SubType == ControlType.Toggle)
                    (Widget as QPushButton).Checked = value;
            }
        }
		
        protected override ControlType DefaultSubtype {
            get { return ControlType.Check; }
        }
        
        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.Toggle };
            }
        }

        protected override QObject Construct ()
        {
            if(Original.SubType == ControlType.Check)
                return new QCheckBox(Original.Label);
            else if(Original.SubType == ControlType.Toggle)
            {
                var Ret = new QPushButton(Original.Label);
                Ret.Checkable = true;
                return Ret;
            }
            
            return null;
        }

        protected override string SignalForType (ControlType Type)
        {
            // Same for both checkbox and togglebutton
            return "toggled(bool)";
        }
    }
}
