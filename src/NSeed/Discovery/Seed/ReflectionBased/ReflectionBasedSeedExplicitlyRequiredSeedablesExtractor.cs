using System;
using NSeed.Discovery.Seedable.ReflectionBased;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedExplicitlyRequiredSeedablesExtractor : BaseSeedExplicitlyRequiredSeedablesExtractor<Type, Type>
    {
        public ReflectionBasedSeedExplicitlyRequiredSeedablesExtractor(ISeedInfoBuilder<Type> seedInfoBuilder)
            : base(new ReflectionBasedExplicitlyRequiredSeedableDiscoverer(),
                   seedInfoBuilder)
        {
        }
    }
}