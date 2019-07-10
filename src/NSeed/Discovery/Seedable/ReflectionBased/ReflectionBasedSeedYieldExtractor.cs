using System;
using NSeed.Discovery.Yield.ReflectionBased;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedYieldExtractor : BaseSeedYieldExtractor<Type, Type>
    {
        public ReflectionBasedSeedYieldExtractor()
            : base(new ReflectionBasedYieldInSeedDiscoverer(),
                   new ReflectionBasedYieldInfoBuilder())
        {
        }
    }
}