using NSeed.MetaInfo;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Discovery.Seedable
{
    internal abstract class BaseExplicitlyRequiredSeedablesExtractor<TSourceSeedableImplementation, TSeedableImplementation> : IExplicitlyRequiredSeedablesExtractor<TSourceSeedableImplementation>
        where TSourceSeedableImplementation : class
        where TSeedableImplementation : class
    {
        private readonly IExplicitlyRequiredSeedablesDiscoverer<TSourceSeedableImplementation, TSeedableImplementation> explicitlyRequiredSeedableDiscoverer;
        private readonly ISeedableInfoBuilder<TSeedableImplementation> seedableBuilder;

        protected internal BaseExplicitlyRequiredSeedablesExtractor(
            IExplicitlyRequiredSeedablesDiscoverer<TSourceSeedableImplementation, TSeedableImplementation> explicitlyRequiredSeedableDiscoverer,
            ISeedableInfoBuilder<TSeedableImplementation> seedableBuilder)
        {
            System.Diagnostics.Debug.Assert(explicitlyRequiredSeedableDiscoverer != null);
            System.Diagnostics.Debug.Assert(seedableBuilder != null);

            this.explicitlyRequiredSeedableDiscoverer = explicitlyRequiredSeedableDiscoverer;
            this.seedableBuilder = seedableBuilder;
        }

        IReadOnlyCollection<SeedableInfo> IExtractor<TSourceSeedableImplementation, IReadOnlyCollection<SeedableInfo>>.ExtractFrom(TSourceSeedableImplementation seedableImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedableImplementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return explicitlyRequiredSeedableDiscoverer.DiscoverIn(seedableImplementation)
                                .DiscoveredItems
                                .Select(seedable => seedableBuilder.BuildFrom(seedable))
                                .Where(seedable => seedable != null) // Ignore circular dependencies.
                                .ToArray();
        }
    }
}
