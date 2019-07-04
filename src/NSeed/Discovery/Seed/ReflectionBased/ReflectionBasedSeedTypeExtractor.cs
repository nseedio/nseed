using System;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedTypeExtractor : ISeedTypeExtractor<Type>
    {
        Type IExtractor<Type, Type>.ExtractFrom(Type seedImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return seedImplementation;

            // TODO-IG: Collect errors.
        }
    }
}