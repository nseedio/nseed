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

        internal BaseSeedInfoBuilder(ISeedTypeExtractor<TSeedImplementation> typeExtractor,
                                     ISeedFullNameExtractor<TSeedImplementation> fullNameExtractor,
                                     ISeedFriendlyNameExtractor<TSeedImplementation> friendlyNameExtractor,
                                     ISeedDescriptionExtractor<TSeedImplementation> descriptionExtractor,
                                     ISeedEntitiesExtractor<TSeedImplementation> entitiesExtractor)
        {
            typeExtractor.MustNotBeNull(nameof(typeExtractor));
            fullNameExtractor.MustNotBeNull(nameof(fullNameExtractor));
            friendlyNameExtractor.MustNotBeNull(nameof(friendlyNameExtractor));
            descriptionExtractor.MustNotBeNull(nameof(descriptionExtractor));
            entitiesExtractor.MustNotBeNull(nameof(entitiesExtractor));

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
            this.entitiesExtractor = entitiesExtractor;
        }

        SeedInfo IMetaInfoBuilder<TSeedImplementation, SeedInfo>.BuildFrom(TSeedImplementation implementation)
        {
            implementation.MustNotBeNull(nameof(implementation));

            Type type = typeExtractor.ExtractFrom(implementation);
            string fullName = fullNameExtractor.ExtractFrom(implementation);
            string friendlyName = friendlyNameExtractor.ExtractFrom(implementation);
            string description = descriptionExtractor.ExtractFrom(implementation);
            var entities = entitiesExtractor.ExtractFrom(implementation);

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