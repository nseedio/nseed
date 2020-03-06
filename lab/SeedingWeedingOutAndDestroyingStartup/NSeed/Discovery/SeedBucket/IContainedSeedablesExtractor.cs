using NSeed.MetaInfo;
using System.Collections.Generic;

namespace NSeed.Discovery.SeedBucket
{
    internal interface IContainedSeedablesExtractor<TSeedBucketImplementation> : IExtractor<TSeedBucketImplementation, IReadOnlyCollection<SeedableInfo>>
        where TSeedBucketImplementation : class
    {
    }
}
