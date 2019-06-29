namespace NSeed.Discovery.Seed
{
    internal interface ISeedFriendlyNameExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, string>
        where TSeedImplementation : class
    {
    }
}