using NSeed.MetaInfo;
using System.Collections.Generic;

namespace NSeed.Discovery.Seedable
{
    /// <remarks>
    /// Only seeds can require yields. Scenarios cannot.
    /// That's why "Seed" in the name of the interface and not "Seedable".
    /// </remarks>
    internal interface ISeedRequiredYieldsExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, IReadOnlyCollection<RequiredYieldInfo>>
        where TSeedImplementation : class
    {
    }
}
