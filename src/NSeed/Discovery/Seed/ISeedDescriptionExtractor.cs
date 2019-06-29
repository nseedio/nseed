namespace NSeed.Discovery.Seed
{
    internal interface ISeedDescriptionExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, string>
        where TSeedImplementation : class
    {
    }
}