using System;

namespace NSeed.Discovery
{
    internal interface ITypeExtractor<TSource> : IExtractor<TSource, Type?>
        where TSource : class
    {
    }
}
