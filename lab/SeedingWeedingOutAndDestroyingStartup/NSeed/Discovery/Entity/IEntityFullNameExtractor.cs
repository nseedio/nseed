namespace NSeed.Discovery.Entity
{
    internal interface IEntityFullNameExtractor<TEntityImplementation> : IFullNameExtractor<TEntityImplementation>
        where TEntityImplementation : class
    {
    }
}
