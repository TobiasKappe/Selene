
using System;

namespace Selene.Backend
{
    public interface IValidatable<T> 
    {
        IValidator<T> Validator { set; }
    }
}
