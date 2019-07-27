using NSeed.MetaInfo;

namespace NSeed.Discovery.Seedable
{
    internal interface ISeedableInfoBuilder<TSeedableImplementation> : IMetaInfoBuilder<TSeedableImplementation, SeedableInfo?>
        where TSeedableImplementation : class
    {
    }
}
