using NSeed.MetaInfo;

namespace NSeed.Discovery.Seedable
{
    /// <remarks>
    /// Only seeds can have yields. Scenarios cannot.
    /// That's why "Seed" in the name of the interface and not "Seedable".
    /// </remarks>
    internal interface ISeedProvidedYieldExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, ProvidedYieldInfo>
        where TSeedImplementation : class
    {
    }
}