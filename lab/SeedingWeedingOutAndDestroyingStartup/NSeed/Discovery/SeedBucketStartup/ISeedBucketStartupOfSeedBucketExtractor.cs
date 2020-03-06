using NSeed.MetaInfo;
using System.Collections.Generic;

namespace NSeed.Discovery.SeedBucketStartup
{
    internal interface ISeedBucketStartupOfSeedBucketExtractor<TSeedBucketImplementation> : IExtractor<TSeedBucketImplementation, IReadOnlyCollection<SeedBucketStartupInfo>>
        where TSeedBucketImplementation : class
    {
    }
}
