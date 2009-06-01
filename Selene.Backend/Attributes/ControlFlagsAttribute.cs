
using System;

namespace Selene.Backend
{
    public class ControlFlagsAttribute : Attribute
    {
        public object[] Flags;
        
        public ControlFlagsAttribute(params object[] Flags)
        {
            this.Flags = Flags;
        }
    }
}
