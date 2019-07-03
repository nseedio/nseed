using System.Collections.Generic;

namespace NSeed.Discovery
{
    internal class Discovery<TDiscovery>
    {
        public IReadOnlyCollection<TDiscovery> DiscoveredItems { get; }

        public Discovery(IReadOnlyCollection<TDiscovery> discoveredItems)
        {
            System.Diagnostics.Debug.Assert(discoveredItems != null);

            DiscoveredItems = discoveredItems;
        }

        // TODO: Soon we will have Errors here as well.
    }
}
