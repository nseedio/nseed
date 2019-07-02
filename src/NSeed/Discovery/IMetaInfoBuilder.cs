using NSeed.MetaInfo;

namespace NSeed.Discovery
{
    internal interface IMetaInfoBuilder<TImplementation, TMetaInfo>
        where TImplementation : class
        where TMetaInfo : BaseMetaInfo
    {
        TMetaInfo BuildFrom(TImplementation implementation);
    }
}
