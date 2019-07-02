using System;
using NSeed.Guards;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityFullNameExtractor : IEntityFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type entityImplementation)
        {
            entityImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(entityImplementation)));
            System.Diagnostics.Debug.Assert(entityImplementation.IsSeedType());

            return entityImplementation.FullName;
        }
    }
}