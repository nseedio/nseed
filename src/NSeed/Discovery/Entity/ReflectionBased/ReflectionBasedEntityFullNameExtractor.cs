using NSeed.MetaInfo;
using System;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityFullNameExtractor : IEntityFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type entityImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(entityImplementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return entityImplementation.FullName ?? string.Empty;
        }
    }
}
