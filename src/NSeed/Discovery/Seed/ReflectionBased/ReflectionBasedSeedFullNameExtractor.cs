using System;
using Light.GuardClauses;
using NSeed.Guards;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedFullNameExtractor : ISeedFullNameExtractor<Type>
    {
        string ISeedFullNameExtractor<Type>.ExtractFrom(Type seedImplementation)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            return seedImplementation.FullName;
        }
    }
}