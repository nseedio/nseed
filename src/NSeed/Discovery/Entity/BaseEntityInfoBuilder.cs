using NSeed.MetaInfo;
using System;

namespace NSeed.Discovery.Entity
{
    internal abstract class BaseEntityInfoBuilder<TEntityImplementation> : IEntityInfoBuilder<TEntityImplementation>
        where TEntityImplementation : class
    {
        private readonly IEntityTypeExtractor<TEntityImplementation> typeExtractor;
        private readonly IEntityFullNameExtractor<TEntityImplementation> fullNameExtractor;
        private readonly IValidator<TEntityImplementation> validator;
        private readonly IMetaInfoPool<TEntityImplementation, EntityInfo> entityInfoPool;

        internal BaseEntityInfoBuilder(
            IEntityTypeExtractor<TEntityImplementation> typeExtractor,
            IEntityFullNameExtractor<TEntityImplementation> fullNameExtractor,
            IValidator<TEntityImplementation> validator,
            IMetaInfoPool<TEntityImplementation, EntityInfo> entityInfoPool)
        {
            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.validator = validator;
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
            var directErrors = validator.Validate(implementation);

            return new EntityInfo
            (
                type ?? MetaInfo.MetaInfo.UnknownImplementation,
                type,
                fullName,
                directErrors
            );
        }
    }
}
