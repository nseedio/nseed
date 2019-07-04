using NSeed.Guards;
using System.Linq;
using NSeed.MetaInfo;
using System.Collections.Generic;
using NSeed.Discovery.Entity;

namespace NSeed.Discovery.Seed
{
    internal abstract class BaseSeedEntitiesExtractor<TSeedImplementation, TEntityImplementation> : ISeedEntitiesExtractor<TSeedImplementation>
        where TSeedImplementation : class
        where TEntityImplementation : class
    {
        private readonly IEntityInSeedDiscoverer<TSeedImplementation, TEntityImplementation> entityDiscoverer;
        private readonly IEntityInfoBuilder<TEntityImplementation> entityBuilder;

        internal protected BaseSeedEntitiesExtractor(IEntityInSeedDiscoverer<TSeedImplementation, TEntityImplementation> entityDiscoverer,
                                                     IEntityInfoBuilder<TEntityImplementation> entityBuilder)
        {
            entityDiscoverer.MustNotBeNull(nameof(entityDiscoverer));
            entityBuilder.MustNotBeNull(nameof(entityBuilder));

            this.entityDiscoverer = entityDiscoverer;
            this.entityBuilder = entityBuilder;
        }

        IReadOnlyCollection<EntityInfo> IExtractor<TSeedImplementation, IReadOnlyCollection<EntityInfo>>.ExtractFrom(TSeedImplementation seedImplementation, IErrorCollector errorCollector)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));

            return entityDiscoverer.DiscoverIn(seedImplementation)
                                .DiscoveredItems
                                .Select(entityType => entityBuilder.BuildFrom(entityType))
                                .ToArray();

            // TODO-IG: Collect errors.
        }
    }
}