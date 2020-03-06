using NSeed.MetaInfo;

namespace NSeed.Discovery.Yield
{
    internal interface IProvidedYieldInfoBuilder<TYieldImplementation> : IMetaInfoBuilder<TYieldImplementation, ProvidedYieldInfo>
        where TYieldImplementation : class
    {
    }
}
