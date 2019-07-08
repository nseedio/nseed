using NSeed.MetaInfo;
using System.Collections.Generic;

namespace NSeed.Discovery.Seedable
{
    /// <remarks>
    /// Only seeds can have entities. Scenarios cannot.
    /// </remarks>
    internal interface ISeedEntitiesExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, IReadOnlyCollection<EntityInfo>>
        where TSeedImplementation : class
    {
    }
}