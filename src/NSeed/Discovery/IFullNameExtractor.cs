namespace NSeed.Discovery
{
    internal interface IFullNameExtractor<TSource> : IExtractor<TSource, string>
        where TSource : class
    {
    }
}
