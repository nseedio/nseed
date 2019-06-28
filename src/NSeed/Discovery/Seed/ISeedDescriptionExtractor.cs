namespace NSeed.Discovery.Seed
{
    internal interface ISeedDescriptionExtractor<TSeedImplementation>
        where TSeedImplementation : class
    {
        string ExtractFrom(TSeedImplementation seedImplementation);
    }
}