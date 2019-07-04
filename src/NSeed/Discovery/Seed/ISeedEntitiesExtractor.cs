using NSeed.MetaInfo;
using System.Collections.Generic;

namespace NSeed.Discovery.Seed
{
    internal interface ISeedEntitiesExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, IReadOnlyCollection<EntityInfo>>
        where TSeedImplementation : class
    {
    }
}