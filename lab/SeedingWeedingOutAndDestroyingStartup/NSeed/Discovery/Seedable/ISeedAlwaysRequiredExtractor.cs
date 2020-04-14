namespace NSeed.Discovery.Seedable
{
    internal interface ISeedAlwaysRequiredExtractor<TSource> : IExtractor<TSource, bool>
        where TSource : class
    {
    }
}
