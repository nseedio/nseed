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
        private readonly IMetaInfoPool<TEntityImplementation, EntityInfo> entityInfoPool;

        internal BaseEntityInfoBuilder(IEntityTypeExtractor<TEntityImplementation> typeExtractor,
                                       IEntityFullNameExtractor<TEntityImplementation> fullNameExtractor,
                                       IMetaInfoPool<TEntityImplementation, EntityInfo> entityInfoPool)
        {
            typeExtractor.MustNotBeNull(nameof(typeExtractor));
            fullNameExtractor.MustNotBeNull(nameof(fullNameExtractor));
            entityInfoPool.MustNotBeNull(nameof(entityInfoPool));

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.entityInfoPool = entityInfoPool;
        }

        EntityInfo IMetaInfoBuilder<TEntityImplementation, EntityInfo>.BuildFrom(TEntityImplementation implementation)
        {
            implementation.MustNotBeNull(nameof(implementation));

            return entityInfoPool.GetOrAdd(implementation, CreateEntityInfo);
        }

        private EntityInfo CreateEntityInfo(TEntityImplementation implementation)
        {
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