using System;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityFullNameExtractor : IEntityFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type entityImplementation)
        {
            return entityImplementation.FullName ?? string.Empty;
        }
    }
}
