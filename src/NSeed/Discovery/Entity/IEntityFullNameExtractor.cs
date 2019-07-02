namespace NSeed.Discovery
{
    internal interface IEntityFullNameExtractor<TEntityImplementation> : IFullNameExtractor<TEntityImplementation>
        where TEntityImplementation : class
    {
    }
}