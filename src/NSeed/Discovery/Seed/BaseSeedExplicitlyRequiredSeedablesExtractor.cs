using NSeed.Guards;
using System.Linq;
using NSeed.MetaInfo;
using System.Collections.Generic;
using NSeed.Discovery.Seedable;

namespace NSeed.Discovery.Seed
{
    internal abstract class BaseSeedExplicitlyRequiredSeedablesExtractor<TSeedImplementation, TSeedableImplementation> : ISeedExplicitlyRequiredSeedablesExtractor<TSeedImplementation>
        where TSeedImplementation : class
        where TSeedableImplementation : class
    {
        private readonly IExplicitlyRequiredSeedableDiscoverer<TSeedImplementation, TSeedableImplementation> explicitlyRequiredSeedableDiscoverer;
        private readonly ISeedInfoBuilder<TSeedableImplementation> seedBuilder;

        internal protected BaseSeedExplicitlyRequiredSeedablesExtractor(
            IExplicitlyRequiredSeedableDiscoverer<TSeedImplementation, TSeedableImplementation> explicitlyRequiredSeedableDiscoverer,
            ISeedInfoBuilder<TSeedableImplementation> seedBuilder)
        {
            explicitlyRequiredSeedableDiscoverer.MustNotBeNull(nameof(explicitlyRequiredSeedableDiscoverer));
            seedBuilder.MustNotBeNull(nameof(seedBuilder));

            this.explicitlyRequiredSeedableDiscoverer = explicitlyRequiredSeedableDiscoverer;
            this.seedBuilder = seedBuilder;
        }

        IReadOnlyCollection<SeedableInfo> IExtractor<TSeedImplementation, IReadOnlyCollection<SeedableInfo>>.ExtractFrom(TSeedImplementation seedImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedImplementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return explicitlyRequiredSeedableDiscoverer.DiscoverIn(seedImplementation)
                                .DiscoveredItems
                                // TODO-IG: Add implementation for IScenario.
                                .Select(entityType => seedBuilder.BuildFrom(entityType))
                                .ToArray();

            // TODO-IG: Collect errors.
        }
    }
}