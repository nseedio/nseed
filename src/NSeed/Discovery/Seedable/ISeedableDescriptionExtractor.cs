namespace NSeed.Discovery.Seedable
{
    internal interface ISeedableDescriptionExtractor<TSeedableImplementation> : IExtractor<TSeedableImplementation, string>
        where TSeedableImplementation : class
    {
    }
}