using NSeed.MetaInfo;
using System.Collections.Generic;

namespace NSeed.Discovery.Seedable
{
    internal interface IExplicitlyRequiredSeedablesExtractor<TSourceSeedableImplementation> : IExtractor<TSourceSeedableImplementation, IReadOnlyCollection<SeedableInfo>>
        where TSourceSeedableImplementation : class
    {
    }
}