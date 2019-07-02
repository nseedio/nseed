using System;
using NSeed.Guards;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityFullNameExtractor : IEntityFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type entityImplementation)
        {
            entityImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(entityImplementation)));

            return entityImplementation.FullName ?? string.Empty;
        }
    }
}