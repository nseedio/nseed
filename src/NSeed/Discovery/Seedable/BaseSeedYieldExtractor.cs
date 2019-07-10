using NSeed.Guards;
using System.Linq;
using NSeed.MetaInfo;
using NSeed.Discovery.Yield;

namespace NSeed.Discovery.Seedable
{
    internal abstract class BaseSeedYieldExtractor<TSeedImplementation, TYieldImplementation> : ISeedYieldExtractor<TSeedImplementation>
        where TSeedImplementation : class
        where TYieldImplementation : class
    {
        private readonly IYieldInSeedDiscoverer<TSeedImplementation, TYieldImplementation> yieldDiscoverer;
        private readonly IYieldInfoBuilder<TYieldImplementation> yieldBuilder;

        internal protected BaseSeedYieldExtractor(IYieldInSeedDiscoverer<TSeedImplementation, TYieldImplementation> yieldDiscoverer,
                                                  IYieldInfoBuilder<TYieldImplementation> yieldBuilder)
        {
            yieldDiscoverer.MustNotBeNull(nameof(yieldDiscoverer));
            yieldBuilder.MustNotBeNull(nameof(yieldBuilder));

            this.yieldDiscoverer = yieldDiscoverer;
            this.yieldBuilder = yieldBuilder;
        }

        YieldInfo IExtractor<TSeedImplementation, YieldInfo>.ExtractFrom(TSeedImplementation seedImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedImplementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            var discoveredYields = yieldDiscoverer.DiscoverIn(seedImplementation).DiscoveredItems;

            if (discoveredYields.Count != 1) return null;

            return yieldBuilder.BuildFrom(discoveredYields.First());
        }
    }
}