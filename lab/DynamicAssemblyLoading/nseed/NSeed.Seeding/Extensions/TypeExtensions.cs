using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace NSeed.Seeding.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsSeedType(this Type type)
        {
            Debug.Assert(type != null);

            return !type.IsAbstract && type.IsSealed && type.GetInterfaces().Contains(typeof(ISeed));
        }

        public static bool IsSeedingSetupType(this Type type)
        {
            Debug.Assert(type != null);

            return !type.IsAbstract && 
                   type.GetInterfaces().Contains(typeof(ISeedingSetup)) &&
                   type.HasParameterlessConstructor();
        }

        private static readonly Type[] EmptyTypeArray = new Type[0];
        public static bool HasParameterlessConstructor(this Type type)
        {
            Debug.Assert(type != null);

            return type.GetConstructor(EmptyTypeArray) != null;
        }

        public static bool IsSeedOutputProperty(this PropertyInfo propertyInfo)
        {
            Debug.Assert(propertyInfo != null);

            return propertyInfo.CanRead && propertyInfo.CanWrite &&
                   PropertyTypeIsSeedOutputType(propertyInfo.PropertyType);

            bool PropertyTypeIsSeedOutputType(Type propertyType)
            {
                return propertyType.Name == "Output" &&
                       propertyType.IsClass &&
                       propertyType.IsAbstract == false &&
                       // It must not be a static class, which means !IsAbstract && !IsSealed,
                       // but we already checked for !IsAbstract so there is no need for additional check.
                       propertyType.IsNestedPublic &&
                       propertyType.DeclaringType.IsSeedType();
            }
        }
    }
}