namespace NSeed.Discovery.Seed
{
    internal interface ISeedTypeExtractor<TSeedImplementation> : ITypeExtractor<TSeedImplementation>
        where TSeedImplementation : class
    {
    }
}