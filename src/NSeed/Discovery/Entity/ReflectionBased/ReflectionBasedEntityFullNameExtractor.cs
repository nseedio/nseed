using System;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityFullNameExtractor : IEntityFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type entityImplementation, IErrorCollector errorCollector)
        {
            entityImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(entityImplementation)));

            return entityImplementation.FullName ?? string.Empty;

            // TODO-IG: Collect errors.
        }
    }
}