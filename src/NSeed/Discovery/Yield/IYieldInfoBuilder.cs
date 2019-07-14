using NSeed.MetaInfo;

namespace NSeed.Discovery.Yield
{
    internal interface IYieldInfoBuilder<TYieldImplementation> : IMetaInfoBuilder<TYieldImplementation, ProvidedYieldInfo>
        where TYieldImplementation : class
    {
    }
}
