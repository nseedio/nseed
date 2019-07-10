namespace NSeed.Discovery.Yield
{
    internal interface IYieldFullNameExtractor<TYieldImplementation> : IFullNameExtractor<TYieldImplementation>
        where TYieldImplementation : class
    {
    }
}