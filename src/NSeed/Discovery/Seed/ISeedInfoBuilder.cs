using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed
{
    internal interface ISeedInfoBuilder<TSeedImplementation>
        where TSeedImplementation : class
    {
        SeedInfo BuildSeedInfoFrom(TSeedImplementation seedImplementation);
    }
}
