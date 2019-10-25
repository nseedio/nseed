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
            this.explicitlyRequiredSeedableDiscoverer = explicitlyRequiredSeedableDiscoverer;
            this.seedableBuilder = seedableBuilder;
        }

        IReadOnlyCollection<SeedableInfo> IExtractor<TSourceSeedableImplementation, IReadOnlyCollection<SeedableInfo>>.ExtractFrom(TSourceSeedableImplementation seedableImplementation)
        {
            // CS8619:
            // The compiler is not able to figure out that we filter out nulls
            // with the "Where(seedable => seedable != null)".
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return explicitlyRequiredSeedableDiscoverer.DiscoverIn(seedableImplementation)
                                .DiscoveredItems
                                .Select(seedable => seedableBuilder.BuildFrom(seedable))
                                .Where(seedable => seedable != null) // Ignore circular dependencies.
                                .ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    }
}
