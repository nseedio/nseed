using NSeed.Discovery.Entity.ReflectionBased;
using System;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedEntitiesExtractor : BaseSeedEntitiesExtractor<Type, Type>
    {
        public ReflectionBasedSeedEntitiesExtractor()
            : base(
                  new ReflectionBasedEntityInSeedDiscoverer(),
                  new ReflectionBasedEntityInfoBuilder())
        {
        }
    }
}
