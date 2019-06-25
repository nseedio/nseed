using System;
using NSeed.Guards;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedTypeExtractor : ISeedTypeExtractor<Type>
    {
        Type ISeedTypeExtractor<Type>.ExtractFrom(Type seedImplementation)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            return seedImplementation;
        }
    }
}