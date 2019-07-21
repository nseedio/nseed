namespace NSeed.Discovery
{
    internal interface IFriendlyNameExtractor<TSource> : IExtractor<TSource, string>
        where TSource : class
    {
    }
}
