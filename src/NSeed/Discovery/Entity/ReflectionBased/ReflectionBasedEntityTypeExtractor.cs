using System;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityTypeExtractor : IEntityTypeExtractor<Type>
    {
        Type IExtractor<Type, Type>.ExtractFrom(Type entityImplementation, IErrorCollector errorCollector)
        {
            entityImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(entityImplementation)));

            return entityImplementation.IsGenericParameter
                ? null
                : entityImplementation;

            // TODO-IG: Collect errors.
        }
    }
}