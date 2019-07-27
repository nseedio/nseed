using NSeed.MetaInfo;

namespace NSeed.Discovery
{
    internal interface IExtractor<TSource, TExtract>
        where TSource : class
        where TExtract : class?
    {
        TExtract ExtractFrom(TSource source, IErrorCollector errorCollector);
    }
}
