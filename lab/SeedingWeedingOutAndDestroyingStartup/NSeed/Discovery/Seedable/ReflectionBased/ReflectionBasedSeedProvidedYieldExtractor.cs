using NSeed.Discovery.Yield.ReflectionBased;
using System;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedProvidedYieldExtractor : BaseSeedProvidedYieldExtractor<Type, Type>
    {
        public ReflectionBasedSeedProvidedYieldExtractor()
            : base(
                  new ReflectionBasedProvidedYieldInSeedDiscoverer(),
                  new ReflectionBasedProvidedYieldInfoBuilder())
        {
        }
    }
}
