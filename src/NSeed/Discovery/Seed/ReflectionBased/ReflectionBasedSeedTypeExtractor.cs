using System;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedTypeExtractor : ISeedTypeExtractor<Type>
    {
        Type IExtractor<Type, Type>.ExtractFrom(Type seedImplementation, IErrorCollector errorCollector)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            return seedImplementation;

            // TODO-IG: Collect errors.
        }
    }
}