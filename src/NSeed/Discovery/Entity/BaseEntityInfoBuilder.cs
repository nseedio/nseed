using System;
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
            System.Diagnostics.Debug.Assert(typeExtractor != null);
            System.Diagnostics.Debug.Assert(fullNameExtractor != null);
            System.Diagnostics.Debug.Assert(entityInfoPool != null);

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.entityInfoPool = entityInfoPool;
        }

        EntityInfo IMetaInfoBuilder<TEntityImplementation, EntityInfo>.BuildFrom(TEntityImplementation implementation)
        {
            System.Diagnostics.Debug.Assert(implementation != null);

            return entityInfoPool.GetOrAdd(implementation, CreateEntityInfo);
        }

        private EntityInfo CreateEntityInfo(TEntityImplementation implementation)
        {
            var errorCollector = new DistinctErrorCollectorAndProvider();

            Type type = typeExtractor.ExtractFrom(implementation, errorCollector);
            string fullName = fullNameExtractor.ExtractFrom(implementation, errorCollector);

            return new EntityInfo
            (
                type,
                fullName
                // TODO-IG: Errors.
            );
        }
    }
}