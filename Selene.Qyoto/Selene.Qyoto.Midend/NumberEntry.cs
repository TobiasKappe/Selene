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
    public class NumberEntry : QConverterProxy<int>
    {
        protected override int ActualValue {
            get 
            { 
                if(Original.SubType == ControlType.Spin || Original.SubType == ControlType.Default)
                    return (Widget as QSpinBox).Value; 
                else return (Widget as QSlider).Value;
            }
            set 
            { 
                if(Original.SubType == ControlType.Spin || Original.SubType == ControlType.Default)
                    (Widget as QSpinBox).Value = value;
                else (Widget as QSlider).Value = value;
            }
        }
        
        protected override ControlType[] Supported {
            get {
                return new ControlType[] { ControlType.Default, ControlType.Spin, ControlType.Glider };
            }
        }

        protected override QObject Construct ()
        {
            int Min = int.MinValue, Max = int.MaxValue, Step = 1;
            bool Wrap = false;

            Original.GetFlag<int>(0, ref Min);
            Original.GetFlag(1, ref Max);
            Original.GetFlag(2, ref Step);
            Original.GetFlag(0, ref Wrap);
            
            if(Original.SubType == ControlType.Spin || Original.SubType == ControlType.Default)
            {
                QSpinBox Ret = new QSpinBox();
                Ret.Maximum = Max;
                Ret.Minimum = Min;
                Ret.Wrapping = Wrap;
                Ret.SingleStep = Step;
                return Ret;
            }
            else if(Original.SubType == ControlType.Glider)
            {
                QSlider Ret = new QSlider(Qt.Orientation.Horizontal);
                Ret.Maximum = Max;
                Ret.Minimum = Min;
                Ret.SingleStep = Step;
                return Ret;
            }

            return null;
        }

        protected override string SignalForType (ControlType Type)
        {
            // Same for both QSpinBox and QSlider
            return "valueChanged(int)";
        }
    }
}
