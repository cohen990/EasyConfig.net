using System;

namespace EasyConfig.Attributes
{
    public class DefaultAttribute : Attribute
    {
        public object Default;

        public DefaultAttribute(object i)
        {
            Default = i;
        }
    }
}