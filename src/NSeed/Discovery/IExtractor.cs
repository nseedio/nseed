namespace NSeed.Discovery
{
    internal interface IExtractor<TSource, TExtract> where TSource : class
    {
        TExtract ExtractFrom(TSource source);
    }
}