using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyConfig.Reflection
{
    public static class MemberInfoReflector
    {
        public static Type GetUnderlyingType(this MemberInfo member)
        {
            if(member == null)
                throw new ArgumentNullException(nameof(member));

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException("Input MemberInfo must be of type FieldInfo or PropertyInfo");
            }
        }

        public static IEnumerable<MemberInfo> GetFieldsAndProperties(MemberInfo[] members)
        {
            if (members == null || members.Length == 0)
                return Enumerable.Empty<MemberInfo>();

                var result = new List<MemberInfo>();

            foreach(var member in members)
            {
                if (member.MemberType.HasFlag(MemberTypes.Field) || member.MemberType.HasFlag(MemberTypes.Property))
                {
                    result.Add(member);
                }
            }

            return result;
        }
    }
}
