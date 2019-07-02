using System;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Entity
{
    internal abstract class BaseEntityInfoBuilder<TEntityImplementation> : IEntityInfoBuilder<TEntityImplementation>
        where TEntityImplementation : class
    {
        private readonly IEntityTypeExtractor<TEntityImplementation> typeExtractor;
        private readonly IEntityFullNameExtractor<TEntityImplementation> fullNameExtractor;

        internal BaseEntityInfoBuilder(IEntityTypeExtractor<TEntityImplementation> typeExtractor,
                                       IEntityFullNameExtractor<TEntityImplementation> fullNameExtractor)
        {
            typeExtractor.MustNotBeNull(nameof(typeExtractor));
            fullNameExtractor.MustNotBeNull(nameof(fullNameExtractor));

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
        }

        EntityInfo IMetaInfoBuilder<TEntityImplementation, EntityInfo>.BuildFrom(TEntityImplementation implementation)
        {
            implementation.MustNotBeNull(nameof(implementation));

            Type type = typeExtractor.ExtractFrom(implementation);
            string fullName = fullNameExtractor.ExtractFrom(implementation);

            return new EntityInfo
                (
                    type,
                    fullName
                );
        }
    }
}