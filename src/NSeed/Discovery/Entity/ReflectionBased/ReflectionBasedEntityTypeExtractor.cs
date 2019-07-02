using System;
using NSeed.Guards;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityTypeExtractor : IEntityTypeExtractor<Type>
    {
        Type IExtractor<Type, Type>.ExtractFrom(Type entityImplementation)
        {
            entityImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(entityImplementation)));

            return entityImplementation.IsGenericParameter
                ? null
                : entityImplementation;
        }
    }
}