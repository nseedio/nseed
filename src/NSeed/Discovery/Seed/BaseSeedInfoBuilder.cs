using System;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed
{
    internal abstract class BaseSeedInfoBuilder<TSeedImplementation> : ISeedInfoBuilder<TSeedImplementation>
        where TSeedImplementation : class
    {
        private readonly ISeedTypeExtractor<TSeedImplementation> seedTypeExtractor;
        private readonly ISeedFullNameExtractor<TSeedImplementation> seedFullNameExtractor;
        private readonly ISeedFriendlyNameExtractor<TSeedImplementation> seedFriendlyNameExtractor;
        private readonly ISeedDescriptionExtractor<TSeedImplementation> seedDescriptionExtractor;

        internal BaseSeedInfoBuilder(ISeedTypeExtractor<TSeedImplementation> seedTypeExtractor,
                                     ISeedFullNameExtractor<TSeedImplementation> seedFullNameExtractor,
                                     ISeedFriendlyNameExtractor<TSeedImplementation> seedFriendlyNameExtractor,
                                     ISeedDescriptionExtractor<TSeedImplementation> seedDescriptionExtractor)
        {
            seedTypeExtractor.MustNotBeNull(nameof(seedTypeExtractor));
            seedFullNameExtractor.MustNotBeNull(nameof(seedFullNameExtractor));
            seedFriendlyNameExtractor.MustNotBeNull(nameof(seedFriendlyNameExtractor));
            seedDescriptionExtractor.MustNotBeNull(nameof(seedDescriptionExtractor));

            this.seedTypeExtractor = seedTypeExtractor;
            this.seedFullNameExtractor = seedFullNameExtractor;
            this.seedFriendlyNameExtractor = seedFriendlyNameExtractor;
            this.seedDescriptionExtractor = seedDescriptionExtractor;
        }

        SeedInfo ISeedInfoBuilder<TSeedImplementation>.BuildSeedInfoFrom(TSeedImplementation seedImplementation)
        {
            seedImplementation.MustNotBeNull(nameof(seedImplementation));

            Type type = seedTypeExtractor.ExtractFrom(seedImplementation);
            string fullName = seedFullNameExtractor.ExtractFrom(seedImplementation);
            string friendlyName = seedFriendlyNameExtractor.ExtractFrom(seedImplementation);
            string description = seedDescriptionExtractor.ExtractFrom(seedImplementation);

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