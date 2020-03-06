using NSeed.Discovery.Seedable.ReflectionBased;
using System;

namespace NSeed.Discovery.SeedBucket.ReflectionBased
{
    internal class ReflectionBasedContainedSeedablesExtractor : BaseContainedSeedablesExtractor<Type, Type>
    {
        public ReflectionBasedContainedSeedablesExtractor()
            : base(
                  new ReflectionBasedContainedSeedablesDiscoverer(),
                  new ReflectionBasedSeedableInfoBuilder())
        {
        }
    }
}
