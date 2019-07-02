namespace NSeed.Discovery.Seed
{
    internal interface ISeedFullNameExtractor<TSeedImplementation> : IFullNameExtractor<TSeedImplementation>
        where TSeedImplementation : class
    {
    }
}