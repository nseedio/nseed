using System;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityFullNameExtractor : IEntityFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type entityImplementation)
        {
            System.Diagnostics.Debug.Assert(entityImplementation != null);

            return entityImplementation.FullName ?? string.Empty;
        }
    }
}
