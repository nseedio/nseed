using System.Collections.Generic;
using System.Linq;

namespace NSeed.Discovery
{
    internal class Discovery<TDiscovery>
    {
        public IReadOnlyCollection<TDiscovery> DiscoveredItems { get; }

        public Discovery(IReadOnlyCollection<TDiscovery> discoveredItems)
        {
            System.Diagnostics.Debug.Assert(discoveredItems.All(item => item != null));

            DiscoveredItems = discoveredItems;
        }

        // TODO: Are we going to have Errors here as planned? Hm? If not, there is no need for the Discovery<TDiscovery> class.
    }
}
