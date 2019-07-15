namespace NSeed.Discovery
{
    internal interface IDescriptionExtractor<TSource> : IExtractor<TSource, string>
        where TSource : class
    {
    }
}