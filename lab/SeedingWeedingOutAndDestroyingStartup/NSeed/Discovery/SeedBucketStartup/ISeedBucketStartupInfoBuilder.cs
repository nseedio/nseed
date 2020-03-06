using NSeed.MetaInfo;

namespace NSeed.Discovery.SeedBucketStartup
{
    internal interface ISeedBucketStartupInfoBuilder<TSeedBucketStartupImplementation> : IMetaInfoBuilder<TSeedBucketStartupImplementation, SeedBucketStartupInfo>
        where TSeedBucketStartupImplementation : class
    {
    }
}
