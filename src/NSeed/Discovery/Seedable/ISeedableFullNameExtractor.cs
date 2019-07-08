namespace NSeed.Discovery.Seedable
{
    internal interface ISeedableFullNameExtractor<TSeedableImplementation> : IFullNameExtractor<TSeedableImplementation>
        where TSeedableImplementation : class
    {
    }
}