using NSeed.MetaInfo;
using System.Collections.Generic;

namespace NSeed.Discovery.Seedable
{
    /// <remarks>
    /// Yielded entities can be extracted only from seeds and not from scenarios.
    /// That's why "Seed" in the name of the interface and not "Seedable".
    /// </remarks>
    internal interface ISeedEntitiesExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, IReadOnlyCollection<EntityInfo>>
        where TSeedImplementation : class
    {
    }
}