namespace NSeed.Discovery.Seed
{
    internal interface ISeedFriendlyNameExtractor<TSeedImplementation>
        where TSeedImplementation : class
    {
        string ExtractFrom(TSeedImplementation seedImplementation);
    }
}