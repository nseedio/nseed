using NSeed.MetaInfo;

namespace NSeed.Discovery.Entity
{
    internal interface IEntityInfoBuilder<TEntityImplementation> : IMetaInfoBuilder<TEntityImplementation, EntityInfo>
        where TEntityImplementation : class
    {
    }
}
