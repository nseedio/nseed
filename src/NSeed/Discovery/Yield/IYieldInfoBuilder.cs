using NSeed.MetaInfo;

namespace NSeed.Discovery.Yield
{
    internal interface IYieldInfoBuilder<TYieldImplementation> : IMetaInfoBuilder<TYieldImplementation, YieldInfo>
        where TYieldImplementation : class
    {
    }
}
