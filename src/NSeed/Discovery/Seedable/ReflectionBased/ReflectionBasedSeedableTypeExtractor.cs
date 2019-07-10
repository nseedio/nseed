using System;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedableTypeExtractor : ISeedableTypeExtractor<Type>
    {
        Type IExtractor<Type, Type>.ExtractFrom(Type seedableImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedableImplementation.IsSeedableType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return seedableImplementation;
        }
    }
}