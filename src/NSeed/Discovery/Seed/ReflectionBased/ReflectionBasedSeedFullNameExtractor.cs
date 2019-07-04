using System;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedFullNameExtractor : ISeedFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type seedImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return seedImplementation.FullName;

            // TODO-IG: Collect errors.
        }
    }
}