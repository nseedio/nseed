using NSeed.MetaInfo;
using System;
using System.Linq;

namespace NSeed.Discovery.Entity
{
    internal abstract class BaseEntityInfoBuilder<TEntityImplementation> : IEntityInfoBuilder<TEntityImplementation>
        where TEntityImplementation : class
    {
        private readonly IEntityTypeExtractor<TEntityImplementation> typeExtractor;
        private readonly IEntityFullNameExtractor<TEntityImplementation> fullNameExtractor;
        private readonly IMetaInfoPool<TEntityImplementation, EntityInfo> entityInfoPool;

        internal BaseEntityInfoBuilder(
            IEntityTypeExtractor<TEntityImplementation> typeExtractor,
            IEntityFullNameExtractor<TEntityImplementation> fullNameExtractor,
            IMetaInfoPool<TEntityImplementation, EntityInfo> entityInfoPool)
        {
            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.entityInfoPool = entityInfoPool;
        }

        EntityInfo IMetaInfoBuilder<TEntityImplementation, EntityInfo>.BuildFrom(TEntityImplementation implementation)
        {
            return entityInfoPool.GetOrAdd(implementation, CreateEntityInfo);
        }

        private EntityInfo CreateEntityInfo(TEntityImplementation implementation)
        {
            Type? type = typeExtractor.ExtractFrom(implementation);
            string fullName = fullNameExtractor.ExtractFrom(implementation);

            return new EntityInfo
            (
                type ?? MetaInfo.MetaInfo.UnknownImplementation,
                type,
                fullName,
                Array.Empty<Error>()
            );
        }
    }
}
