using System;
using System.Collections.Generic;

namespace EasyConfig.Exceptions
{
    public class TypeNotSupportedException : Exception
    {
        public TypeNotSupportedException( Type type)
            : base($"The type '{type}' is not supported. Register support for it by calling Config.RegisterTypeSupport.")
        {
        }
    }
}