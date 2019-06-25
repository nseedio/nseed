namespace NSeed.Discovery.Seed
{
    internal interface ISeedFullNameExtractor<TSeedImplementation>
        where TSeedImplementation : class
    {
        string ExtractFrom(TSeedImplementation seedImplementation);
    }
}