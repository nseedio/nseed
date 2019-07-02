using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed
{
    internal interface ISeedInfoBuilder<TSeedImplementation> : IMetaInfoBuilder<TSeedImplementation, SeedInfo>
        where TSeedImplementation : class
    {
    }
}
