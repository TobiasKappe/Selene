using System;

namespace Selene.Backend
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ControlIgnoreAttribute : Attribute
    {
        public ControlIgnoreAttribute()
        {
            
        }
    }
}
