using System;
using NSeed.Discovery.Yield.ReflectionBased;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedProvidedYieldExtractor : BaseSeedProvidedYieldExtractor<Type, Type>
    {
        public ReflectionBasedSeedProvidedYieldExtractor()
            : base(new ReflectionBasedProvidedYieldInSeedDiscoverer(),
                   new ReflectionBasedProvidedYieldInfoBuilder())
        {
        }
    }
}