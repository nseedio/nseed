namespace NSeed.Discovery.Seedable
{
    internal interface ISeedableTypeExtractor<TSeedableImplementation> : ITypeExtractor<TSeedableImplementation>
        where TSeedableImplementation : class
    {
    }
}