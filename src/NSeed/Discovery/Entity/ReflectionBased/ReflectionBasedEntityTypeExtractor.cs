using System;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityTypeExtractor : IEntityTypeExtractor<Type>
    {
        Type? IExtractor<Type, Type?>.ExtractFrom(Type entityImplementation)
        {
            System.Diagnostics.Debug.Assert(entityImplementation != null);

            return entityImplementation.IsGenericParameter
                ? null
                : entityImplementation;
        }
    }
}
