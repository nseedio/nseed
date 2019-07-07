using NSeed.MetaInfo;
using System.Collections.Generic;

namespace NSeed.Discovery.Seed
{
    internal interface ISeedExplicitlyRequiredSeedablesExtractor<TSeedImplementation> : IExtractor<TSeedImplementation, IReadOnlyCollection<SeedableInfo>>
        where TSeedImplementation : class
    {
    }
}