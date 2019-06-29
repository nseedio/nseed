namespace NSeed.Discovery.Seed
{
    internal interface ISeedFullNameExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, string>
        where TSeedImplementation : class
    {
    }
}