using NSeed.MetaInfo;

namespace NSeed.Discovery.Seedable
{
    /// <remarks>
    /// Only seeds can have yields. Scenarios cannot.
    /// </remarks>
    internal interface ISeedYieldExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, ProvidedYieldInfo>
        where TSeedImplementation : class
    {
    }
}