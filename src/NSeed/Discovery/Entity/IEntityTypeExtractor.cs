namespace NSeed.Discovery.Entity
{
    internal interface IEntityTypeExtractor<TEntityImplementation> : ITypeExtractor<TEntityImplementation>
        where TEntityImplementation : class
    {
    }
}