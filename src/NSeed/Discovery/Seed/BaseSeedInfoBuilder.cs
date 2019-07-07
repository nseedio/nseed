using System;
using NSeed.Guards;
using NSeed.MetaInfo;

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
        private readonly ISeedEntitiesExtractor<TSeedImplementation> entitiesExtractor;
        private readonly ISeedExplicitlyRequiredSeedablesExtractor<TSeedImplementation> explicitlyRequiredSeedablesExtractor;
        private readonly IMetaInfoPool<TSeedImplementation, SeedInfo> seedInfoPool;

        internal BaseSeedInfoBuilder(ISeedTypeExtractor<TSeedImplementation> typeExtractor,
                                     ISeedFullNameExtractor<TSeedImplementation> fullNameExtractor,
                                     ISeedFriendlyNameExtractor<TSeedImplementation> friendlyNameExtractor,
                                     ISeedDescriptionExtractor<TSeedImplementation> descriptionExtractor,
                                     ISeedEntitiesExtractor<TSeedImplementation> entitiesExtractor,
                                     Func<ISeedInfoBuilder<TSeedImplementation>, ISeedExplicitlyRequiredSeedablesExtractor<TSeedImplementation>> explicitlyRequiredSeedablesExtractorFactory,
                                     IMetaInfoPool<TSeedImplementation, SeedInfo> seedInfoPool)
        {
            typeExtractor.MustNotBeNull(nameof(typeExtractor));
            fullNameExtractor.MustNotBeNull(nameof(fullNameExtractor));
            friendlyNameExtractor.MustNotBeNull(nameof(friendlyNameExtractor));
            descriptionExtractor.MustNotBeNull(nameof(descriptionExtractor));
            entitiesExtractor.MustNotBeNull(nameof(entitiesExtractor));
            explicitlyRequiredSeedablesExtractorFactory.MustNotBeNull(nameof(explicitlyRequiredSeedablesExtractorFactory));
            seedInfoPool.MustNotBeNull(nameof(seedInfoPool));

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
            this.entitiesExtractor = entitiesExtractor;
            explicitlyRequiredSeedablesExtractor = explicitlyRequiredSeedablesExtractorFactory(this);
            this.seedInfoPool = seedInfoPool;
        }

        SeedInfo IMetaInfoBuilder<TSeedImplementation, SeedInfo>.BuildFrom(TSeedImplementation implementation)
        {
            implementation.MustNotBeNull(nameof(implementation));

            return seedInfoPool.GetOrAdd(implementation, CreateSeedInfo);
        }

        private SeedInfo CreateSeedInfo(TSeedImplementation implementation)
        {
            var errorCollector = new DistinctErrorCollectorAndProvider();

            Type type = typeExtractor.ExtractFrom(implementation, errorCollector);
            string fullName = fullNameExtractor.ExtractFrom(implementation, errorCollector);
            string friendlyName = friendlyNameExtractor.ExtractFrom(implementation, errorCollector);
            string description = descriptionExtractor.ExtractFrom(implementation, errorCollector);
            var entities = entitiesExtractor.ExtractFrom(implementation, errorCollector);
            var explicitelyRequires = explicitlyRequiredSeedablesExtractor.ExtractFrom(implementation, errorCollector);

            return new SeedInfo
            (
                type,
                fullName,
                friendlyName,
                description,
                explicitelyRequires,
                Array.Empty<SeedInfo>(), // TODO-IG: Add implicitly requires.
                entities
            // TODO-IG: Errors.
            );
        }
    }
}