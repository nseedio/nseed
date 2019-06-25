using System;
using NSeed.Guards;

namespace NSeed.Discovery.Seed
{
    internal abstract class BaseSeedInfoBuilder<TSeedImplementation> : ISeedInfoBuilder<TSeedImplementation>
        where TSeedImplementation : class
    {
        private readonly ISeedTypeExtractor<TSeedImplementation> seedTypeExtractor;
        private readonly ISeedFullNameExtractor<TSeedImplementation> seedFullNameExtractor;
        private readonly ISeedFriendlyNameExtractor<TSeedImplementation> seedFriendlyNameExtractor;

        internal BaseSeedInfoBuilder(ISeedTypeExtractor<TSeedImplementation> seedTypeExtractor,
                                     ISeedFullNameExtractor<TSeedImplementation> seedFullNameExtractor,
                                     ISeedFriendlyNameExtractor<TSeedImplementation> seedFriendlyNameExtractor)
        {
            seedTypeExtractor.MustNotBeNull(nameof(seedTypeExtractor));
            seedFullNameExtractor.MustNotBeNull(nameof(seedFullNameExtractor));
            seedFriendlyNameExtractor.MustNotBeNull(nameof(seedFriendlyNameExtractor));

            this.seedTypeExtractor = seedTypeExtractor;
            this.seedFullNameExtractor = seedFullNameExtractor;
            this.seedFriendlyNameExtractor = seedFriendlyNameExtractor;
        }

        SeedInfo ISeedInfoBuilder<TSeedImplementation>.BuildSeedInfoFrom(TSeedImplementation seedImplementation)
        {
            seedImplementation.MustNotBeNull(nameof(seedImplementation));

            Type type = seedTypeExtractor.ExtractFrom(seedImplementation);
            string fullName = seedFullNameExtractor.ExtractFrom(seedImplementation);
            string friendlyName = seedFriendlyNameExtractor.ExtractFrom(seedImplementation);

            return new SeedInfo
                (
                    type,
                    fullName,
                    friendlyName
                );
        }
    }
}