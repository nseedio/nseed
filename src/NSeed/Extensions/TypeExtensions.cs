using NSeed.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NSeed.Extensions
{
    internal static class TypeExtensions
    {
        private static readonly HashSet<Type> SeedInterfaceTypesWithEntities = new HashSet<Type>
        {
            typeof(ISeed<>),
            typeof(ISeed<,>),
            typeof(ISeed<,,>)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSeedInterfaceTypeWithEntities(this Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);

            return SeedInterfaceTypesWithEntities.Contains(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSeedType(this Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);

            return type.GetInterfaces().Contains(typeof(ISeed));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSeedableType(this Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);

            return type.GetInterfaces().Any(@interface => @interface == typeof(ISeed) || @interface == typeof(IScenario));
        }

        public static bool IsYieldType(this Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);

            if (!(type.IsNested && type.Name == "Yield" && type.BaseType?.IsConstructedGenericType == true)) return false;

            if (type.BaseType.GetGenericTypeDefinition() != typeof(YieldOf<>)) return false;

            return type.DeclaringType?.IsSeedType() == true && type.DeclaringType == type.BaseType.GetGenericArguments()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsYieldTypeOfSeed(this Type type, Type seedType)
        {
            System.Diagnostics.Debug.Assert(type != null);
            System.Diagnostics.Debug.Assert(seedType != null);
            System.Diagnostics.Debug.Assert(seedType.IsSeedType());

            return type.IsYieldType() && type.DeclaringType == seedType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSeedBucketType(this Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);

            return typeof(SeedBucket).IsAssignableFrom(type);
        }
    }
}
