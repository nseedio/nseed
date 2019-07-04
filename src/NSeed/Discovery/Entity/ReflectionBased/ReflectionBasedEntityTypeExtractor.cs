using System;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityTypeExtractor : IEntityTypeExtractor<Type>
    {
        Type IExtractor<Type, Type>.ExtractFrom(Type entityImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(entityImplementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return entityImplementation.IsGenericParameter
                ? null
                : entityImplementation;

            // TODO-IG: Collect errors.
        }
    }
}