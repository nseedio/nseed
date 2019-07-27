namespace NSeed.Discovery
{
    internal interface IMetaInfoBuilder<TImplementation, TMetaInfo>
        where TImplementation : class
        where TMetaInfo : MetaInfo.MetaInfo?
    {
        TMetaInfo BuildFrom(TImplementation implementation);
    }
}
