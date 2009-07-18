
using System;

namespace Selene.Backend
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ControlFlagsAttribute : Attribute
    {
        object[] mFlags;

        public object[] Flags {
            get { return mFlags; }
        }

        public ControlFlagsAttribute(params object[] Flags)
        {
            mFlags = Flags;
        }
    }
}
