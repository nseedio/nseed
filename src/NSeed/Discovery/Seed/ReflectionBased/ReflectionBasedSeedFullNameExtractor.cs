using System;
using NSeed.Guards;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedFullNameExtractor : ISeedFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type seedImplementation)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            return seedImplementation.FullName;
        }
    }
}