using NSeed.MetaInfo;
using System;

namespace NSeed.Discovery.SeedBucketStartup
{
    internal abstract class BaseSeedBucketStartupInfoBuilder<TSeedBucketStartupImplementation> : ISeedBucketStartupInfoBuilder<TSeedBucketStartupImplementation>
        where TSeedBucketStartupImplementation : class
    {
        private readonly ITypeExtractor<TSeedBucketStartupImplementation> typeExtractor;
        private readonly IFullNameExtractor<TSeedBucketStartupImplementation> fullNameExtractor;
        private readonly IFriendlyNameExtractor<TSeedBucketStartupImplementation> friendlyNameExtractor;
        private readonly IDescriptionExtractor<TSeedBucketStartupImplementation> descriptionExtractor;

        internal BaseSeedBucketStartupInfoBuilder(
            ITypeExtractor<TSeedBucketStartupImplementation> typeExtractor,
            IFullNameExtractor<TSeedBucketStartupImplementation> fullNameExtractor,
            IFriendlyNameExtractor<TSeedBucketStartupImplementation> friendlyNameExtractor,
            IDescriptionExtractor<TSeedBucketStartupImplementation> descriptionExtractor)
        {
            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
        }

        SeedBucketStartupInfo IMetaInfoBuilder<TSeedBucketStartupImplementation, SeedBucketStartupInfo>.BuildFrom(TSeedBucketStartupImplementation implementation)
        {
            Type? type = typeExtractor.ExtractFrom(implementation);
            string fullName = fullNameExtractor.ExtractFrom(implementation);
            string friendlyName = friendlyNameExtractor.ExtractFrom(implementation);
            string description = descriptionExtractor.ExtractFrom(implementation);

            return new SeedBucketStartupInfo
            (
                implementation,
                type,
                fullName,
                friendlyName,
                description,
                Array.Empty<Error>()
            );
        }
    }
}
