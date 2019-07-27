using NSeed.Discovery.Yield;
using NSeed.MetaInfo;
using System.Linq;

namespace NSeed.Discovery.Seedable
{
    internal abstract class BaseSeedProvidedYieldExtractor<TSeedImplementation, TYieldImplementation> : ISeedProvidedYieldExtractor<TSeedImplementation>
        where TSeedImplementation : class
        where TYieldImplementation : class
    {
        private readonly IProvidedYieldInSeedDiscoverer<TSeedImplementation, TYieldImplementation> yieldDiscoverer;
        private readonly IProvidedYieldInfoBuilder<TYieldImplementation> yieldBuilder;

        protected internal BaseSeedProvidedYieldExtractor(
            IProvidedYieldInSeedDiscoverer<TSeedImplementation, TYieldImplementation> yieldDiscoverer,
            IProvidedYieldInfoBuilder<TYieldImplementation> yieldBuilder)
        {
            System.Diagnostics.Debug.Assert(yieldDiscoverer != null);
            System.Diagnostics.Debug.Assert(yieldBuilder != null);

            this.yieldDiscoverer = yieldDiscoverer;
            this.yieldBuilder = yieldBuilder;
        }

        ProvidedYieldInfo? IExtractor<TSeedImplementation, ProvidedYieldInfo?>.ExtractFrom(TSeedImplementation seedImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedImplementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            var discoveredYields = yieldDiscoverer.DiscoverIn(seedImplementation).DiscoveredItems;

            if (discoveredYields.Count != 1) return null;

            return yieldBuilder.BuildFrom(discoveredYields.First());
        }
    }
}
