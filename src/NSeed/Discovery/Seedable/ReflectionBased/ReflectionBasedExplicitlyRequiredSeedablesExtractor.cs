using System;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedExplicitlyRequiredSeedablesExtractor : BaseExplicitlyRequiredSeedablesExtractor<Type, Type>
    {
        public ReflectionBasedExplicitlyRequiredSeedablesExtractor(ISeedableInfoBuilder<Type> seedableInfoBuilder)
            : base(new ReflectionBasedExplicitlyRequiredSeedablesDiscoverer(),
                   seedableInfoBuilder)
        {
        }
    }
}