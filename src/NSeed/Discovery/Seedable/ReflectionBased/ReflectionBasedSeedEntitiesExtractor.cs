using System;
using NSeed.Discovery.Entity.ReflectionBased;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedEntitiesExtractor : BaseSeedEntitiesExtractor<Type, Type>
    {
        public ReflectionBasedSeedEntitiesExtractor()
            : base(new ReflectionBasedEntityInSeedDiscoverer(),
                   new ReflectionBasedEntityInfoBuilder())
        {
        }
    }
}