
using System;

namespace Selene.Backend
{
    public class InspectionException : Exception
    {
        public Type Culprit;
        
        public InspectionException(Type Culprit, string Description) : base(Description)
        {
            this.Culprit = Culprit;
        }
    }
}
