// SPDX-License-Identifier: GPL-3.0-only
using System.Collections;
using System.ComponentModel;

namespace HMI.Infrastructure.Extensions
{
    public static class ReflectionExtensions
    {
        private static readonly Type GenericCollectionType = typeof(ICollection<>);
        private static readonly Type NonGenericCollectionType = typeof(ICollection);
        
        public static bool IsCollectionType(this Type type)
        {
            return IsNonGenericCollection(type) || IsGenericCollection(type) || ImplementsCollectionInterface(type);
        }
        public static bool IsCollectionProperty(this PropertyDescriptor descriptor)
        {
            Type propertyType = descriptor.PropertyType;
            return propertyType.IsCollectionType();
        }
        public static bool IsEnum(this Type type)
        {
            return type.IsSubclassOf(typeof(Enum));
        }
        public static bool Inherits(this Type toCheck, Type generic)
        {
            while (toCheck != typeof(object))
            {
                Type right = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == right)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
        
        private static bool IsCollectionInterface(Type type)
        {
            return IsNonGenericCollection(type) || IsGenericCollection(type);
        }
        private static bool IsGenericCollection(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == GenericCollectionType;
        }
        private static bool ImplementsCollectionInterface(Type type)
        {
            IEnumerable<Type> interfaces = type.GetInterfaces();

            return interfaces.Any(IsCollectionInterface);
        }
        private static bool IsNonGenericCollection(Type type)
        {
            return type == NonGenericCollectionType;
        }
    }
}
