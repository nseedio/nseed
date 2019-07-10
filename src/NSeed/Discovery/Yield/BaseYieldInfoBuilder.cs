using System;
using NSeed.Guards;
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
            typeExtractor.MustNotBeNull(nameof(typeExtractor));
            fullNameExtractor.MustNotBeNull(nameof(fullNameExtractor));

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
        }

        YieldInfo IMetaInfoBuilder<TYieldImplementation, YieldInfo>.BuildFrom(TYieldImplementation implementation)
        {
            implementation.MustNotBeNull(nameof(implementation));

            var errorCollector = new DistinctErrorCollectorAndProvider();

            Type type = typeExtractor.ExtractFrom(implementation, errorCollector);
            string fullName = fullNameExtractor.ExtractFrom(implementation, errorCollector);

            return new YieldInfo
            (
                type,
                fullName
            // TODO-IG: Errors.
            );
        }
    }
}