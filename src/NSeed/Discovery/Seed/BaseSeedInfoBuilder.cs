using System;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed
{
    internal abstract class BaseSeedInfoBuilder<TSeedImplementation> : ISeedInfoBuilder<TSeedImplementation>
        where TSeedImplementation : class
    {
        private readonly ISeedTypeExtractor<TSeedImplementation> typeExtractor;
        private readonly ISeedFullNameExtractor<TSeedImplementation> fullNameExtractor;
        private readonly ISeedFriendlyNameExtractor<TSeedImplementation> friendlyNameExtractor;
        private readonly ISeedDescriptionExtractor<TSeedImplementation> descriptionExtractor;

        internal BaseSeedInfoBuilder(ISeedTypeExtractor<TSeedImplementation> typeExtractor,
                                     ISeedFullNameExtractor<TSeedImplementation> fullNameExtractor,
                                     ISeedFriendlyNameExtractor<TSeedImplementation> friendlyNameExtractor,
                                     ISeedDescriptionExtractor<TSeedImplementation> descriptionExtractor)
        {
            typeExtractor.MustNotBeNull(nameof(typeExtractor));
            fullNameExtractor.MustNotBeNull(nameof(fullNameExtractor));
            friendlyNameExtractor.MustNotBeNull(nameof(friendlyNameExtractor));
            descriptionExtractor.MustNotBeNull(nameof(descriptionExtractor));

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
        }

        SeedInfo IMetaInfoBuilder<TSeedImplementation, SeedInfo>.BuildFrom(TSeedImplementation implementation)
        {
            implementation.MustNotBeNull(nameof(implementation));

            Type type = typeExtractor.ExtractFrom(implementation);
            string fullName = fullNameExtractor.ExtractFrom(implementation);
            string friendlyName = friendlyNameExtractor.ExtractFrom(implementation);
            string description = descriptionExtractor.ExtractFrom(implementation);

            return new SeedInfo
                (
                    type,
                    fullName,
                    friendlyName,
                    description
                );
        }
    }
}