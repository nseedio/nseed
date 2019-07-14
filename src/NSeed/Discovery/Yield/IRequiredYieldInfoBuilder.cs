using NSeed.MetaInfo;

namespace NSeed.Discovery.Yield
{
    internal interface IRequiredYieldInfoBuilder<TYieldAccessPropertyImplementation> : IMetaInfoBuilder<TYieldAccessPropertyImplementation, RequiredYieldInfo>
        where TYieldAccessPropertyImplementation : class
    {
    }
}
