using System.Linq;
using NSeed.MetaInfo;
using System.Collections.Generic;
using NSeed.Discovery.Entity;

namespace NSeed.Discovery.Seedable
{
    internal abstract class BaseSeedEntitiesExtractor<TSeedImplementation, TEntityImplementation> : ISeedEntitiesExtractor<TSeedImplementation>
        where TSeedImplementation : class
        where TEntityImplementation : class
    {
        private readonly IEntityInSeedDiscoverer<TSeedImplementation, TEntityImplementation> entityDiscoverer;
        private readonly IEntityInfoBuilder<TEntityImplementation> entityBuilder;

        protected internal BaseSeedEntitiesExtractor(IEntityInSeedDiscoverer<TSeedImplementation, TEntityImplementation> entityDiscoverer,
                                                     IEntityInfoBuilder<TEntityImplementation> entityBuilder)
        {
            System.Diagnostics.Debug.Assert(entityDiscoverer != null);
            System.Diagnostics.Debug.Assert(entityBuilder != null);

            this.entityDiscoverer = entityDiscoverer;
            this.entityBuilder = entityBuilder;
        }

        IReadOnlyCollection<EntityInfo> IExtractor<TSeedImplementation, IReadOnlyCollection<EntityInfo>>.ExtractFrom(TSeedImplementation seedImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedImplementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return entityDiscoverer.DiscoverIn(seedImplementation)
                                .DiscoveredItems
                                .Select(entityType => entityBuilder.BuildFrom(entityType))
                                .ToArray();

            // TODO-IG: Collect errors.
        }
    }
}