using System;
using System.Reflection;

namespace Selene.Backend
{
    internal class AttributeHelper
    {
        public static A GetAttribute<A>(MemberInfo F) where A : Attribute
        {
            object[] Attributes = F.GetCustomAttributes(typeof(A), false);

            if(Attributes.Length == 0) return null;
            if(Attributes.Length > 1)
            {
                throw new InspectionException(F.DeclaringType, "Field "+F.Name+" can't have more than one attribute of type "+typeof(A));
            }

            return Attributes[0] as A;
        }
    }
}
