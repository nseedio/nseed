using System;
using NSeed.Guards;
using NSeed.MetaInfo;
using NSeed.Discovery.Entity;
using System.Linq;

namespace NSeed.Discovery.Seed
{
    // TODO-IG: See if we should have only one generic type parameter.
    //          For the moment it seams like all of the parameters will be of the same type.
    //          For reflection based discovery, it will be System.Type.
    //          For source code based discovery most likely always SyntaxNode.
    internal abstract class BaseSeedInfoBuilder<TSeedImplementation, TEntityImplementation> : ISeedInfoBuilder<TSeedImplementation>
        where TSeedImplementation : class
        where TEntityImplementation : class
    {
        private readonly ISeedTypeExtractor<TSeedImplementation> typeExtractor;
        private readonly ISeedFullNameExtractor<TSeedImplementation> fullNameExtractor;
        private readonly ISeedFriendlyNameExtractor<TSeedImplementation> friendlyNameExtractor;
        private readonly ISeedDescriptionExtractor<TSeedImplementation> descriptionExtractor;
        private readonly IEntityInSeedDiscoverer<TSeedImplementation, TEntityImplementation> entityDiscoverer;
        private readonly IEntityInfoBuilder<TEntityImplementation> entityBuilder;

        internal BaseSeedInfoBuilder(ISeedTypeExtractor<TSeedImplementation> typeExtractor,
                                     ISeedFullNameExtractor<TSeedImplementation> fullNameExtractor,
                                     ISeedFriendlyNameExtractor<TSeedImplementation> friendlyNameExtractor,
                                     ISeedDescriptionExtractor<TSeedImplementation> descriptionExtractor,
                                     IEntityInSeedDiscoverer<TSeedImplementation, TEntityImplementation> entityDiscoverer,
                                     IEntityInfoBuilder<TEntityImplementation> entityBuilder)
        {
            typeExtractor.MustNotBeNull(nameof(typeExtractor));
            fullNameExtractor.MustNotBeNull(nameof(fullNameExtractor));
            friendlyNameExtractor.MustNotBeNull(nameof(friendlyNameExtractor));
            descriptionExtractor.MustNotBeNull(nameof(descriptionExtractor));
            entityDiscoverer.MustNotBeNull(nameof(entityDiscoverer));
            entityBuilder.MustNotBeNull(nameof(entityBuilder));

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
            this.entityDiscoverer = entityDiscoverer;
            this.entityBuilder = entityBuilder;
        }

        SeedInfo IMetaInfoBuilder<TSeedImplementation, SeedInfo>.BuildFrom(TSeedImplementation implementation)
        {
            implementation.MustNotBeNull(nameof(implementation));

            Type type = typeExtractor.ExtractFrom(implementation);
            string fullName = fullNameExtractor.ExtractFrom(implementation);
            string friendlyName = friendlyNameExtractor.ExtractFrom(implementation);
            string description = descriptionExtractor.ExtractFrom(implementation);
            var entities = entityDiscoverer.DiscoverIn(implementation)
                                .DiscoveredItems
                                .Select(entityType => entityBuilder.BuildFrom(entityType))
                                .ToArray();

            return new SeedInfo
            (
                type,
                fullName,
                friendlyName,
                description,
                entities
            );
        }
    }
}