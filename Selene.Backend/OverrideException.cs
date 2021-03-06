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
using System.Text;

namespace Selene.Backend
{
    public class OverrideException : Exception
    {
        public OverrideException(Type Target, ControlType Fault, ControlType Default, params ControlType[] Supported) 
            : base(BuildMessage(Target, Fault, Default, Supported))
        {
        }
        
        public OverrideException(Control Orig, ControlType Default, ControlType[] Supported) 
            : base(BuildMessage(Orig.Type, Orig.SubType, Default, Supported))
        {
        }
        
        private static string BuildMessage(Type Target, ControlType Fault, ControlType Default, ControlType[] Supported)
        {
            StringBuilder Alternatives = new StringBuilder();
            Alternatives.Append(string.Format("Override {0} not supported for type {1}, try using ", Fault, Target));
            
            for(int i = 0; i <= Supported.Length; i++)
            {
                if(i == Supported.Length)
                    Alternatives.Append(Default);
                else Alternatives.Append(Supported[i]);
				
                if(i == Supported.Length-1) Alternatives.Append(" or ");
                else if(i < Supported.Length) Alternatives.Append(", ");
            }
            
            return Alternatives.ToString();
        }
    }
}
