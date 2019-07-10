namespace NSeed.Discovery.Yield
{
    internal interface IYieldTypeExtractor<TYieldImplementation> : ITypeExtractor<TYieldImplementation>
        where TYieldImplementation : class
    {
    }
}