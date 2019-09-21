using NSeed.Discovery.Entity;
using NSeed.MetaInfo;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Discovery.Seedable
{
    internal abstract class BaseSeedEntitiesExtractor<TSeedImplementation, TEntityImplementation> : ISeedEntitiesExtractor<TSeedImplementation>
        where TSeedImplementation : class
        where TEntityImplementation : class
    {
        private readonly IEntityInSeedDiscoverer<TSeedImplementation, TEntityImplementation> entityDiscoverer;
        private readonly IEntityInfoBuilder<TEntityImplementation> entityBuilder;

        protected internal BaseSeedEntitiesExtractor(
            IEntityInSeedDiscoverer<TSeedImplementation, TEntityImplementation> entityDiscoverer,
            IEntityInfoBuilder<TEntityImplementation> entityBuilder)
        {
            System.Diagnostics.Debug.Assert(entityDiscoverer != null);
            System.Diagnostics.Debug.Assert(entityBuilder != null);

            this.entityDiscoverer = entityDiscoverer;
            this.entityBuilder = entityBuilder;
        }

        IReadOnlyCollection<EntityInfo> IExtractor<TSeedImplementation, IReadOnlyCollection<EntityInfo>>.ExtractFrom(TSeedImplementation seedImplementation)
        {
            System.Diagnostics.Debug.Assert(seedImplementation != null);

            return entityDiscoverer.DiscoverIn(seedImplementation)
                                .DiscoveredItems

                                // We know that the entity info builder always returns
                                // an entity info and never null; threfore "!".
                                .Select(entityType => entityBuilder.BuildFrom(entityType)!)
                                .ToArray();
        }
    }
}
