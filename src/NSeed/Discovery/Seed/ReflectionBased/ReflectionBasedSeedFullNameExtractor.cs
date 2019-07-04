using System;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedFullNameExtractor : ISeedFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type seedImplementation, IErrorCollector errorCollector)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            return seedImplementation.FullName;

            // TODO-IG: Collect errors.
        }
    }
}