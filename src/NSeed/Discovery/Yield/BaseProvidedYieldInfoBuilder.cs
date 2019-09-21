using NSeed.MetaInfo;
using System;

namespace NSeed.Discovery.Yield
{
    internal abstract class BaseProvidedYieldInfoBuilder<TYieldImplementation> : IProvidedYieldInfoBuilder<TYieldImplementation>
        where TYieldImplementation : class
    {
        private readonly ITypeExtractor<TYieldImplementation> typeExtractor;
        private readonly IFullNameExtractor<TYieldImplementation> fullNameExtractor;

        internal BaseProvidedYieldInfoBuilder(
            ITypeExtractor<TYieldImplementation> typeExtractor,
            IFullNameExtractor<TYieldImplementation> fullNameExtractor)
        {
            System.Diagnostics.Debug.Assert(typeExtractor != null);
            System.Diagnostics.Debug.Assert(fullNameExtractor != null);

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
        }

        ProvidedYieldInfo IMetaInfoBuilder<TYieldImplementation, ProvidedYieldInfo>.BuildFrom(TYieldImplementation implementation)
        {
            System.Diagnostics.Debug.Assert(implementation != null);

            Type? type = typeExtractor.ExtractFrom(implementation);
            string fullName = fullNameExtractor.ExtractFrom(implementation);

            return new ProvidedYieldInfo
            (
                implementation,
                type,
                fullName,
                Array.Empty<Error>()
            );
        }
    }
}
