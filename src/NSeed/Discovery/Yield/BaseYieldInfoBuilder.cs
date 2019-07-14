using System;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Yield
{
    internal abstract class BaseYieldInfoBuilder<TYieldImplementation> : IYieldInfoBuilder<TYieldImplementation>
        where TYieldImplementation : class
    {
        private readonly IYieldTypeExtractor<TYieldImplementation> typeExtractor;
        private readonly IYieldFullNameExtractor<TYieldImplementation> fullNameExtractor;

        internal BaseYieldInfoBuilder(IYieldTypeExtractor<TYieldImplementation> typeExtractor,
                                       IYieldFullNameExtractor<TYieldImplementation> fullNameExtractor)
        {
            System.Diagnostics.Debug.Assert(typeExtractor != null);
            System.Diagnostics.Debug.Assert(fullNameExtractor != null);

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
        }

        ProvidedYieldInfo IMetaInfoBuilder<TYieldImplementation, ProvidedYieldInfo>.BuildFrom(TYieldImplementation implementation)
        {
            System.Diagnostics.Debug.Assert(implementation != null);            

            var errorCollector = new DistinctErrorCollectorAndProvider();

            Type type = typeExtractor.ExtractFrom(implementation, errorCollector);
            string fullName = fullNameExtractor.ExtractFrom(implementation, errorCollector);

            return new ProvidedYieldInfo
            (
                type,
                fullName
            // TODO-IG: Errors.
            );
        }
    }
}