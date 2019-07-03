using NSeed.MetaInfo;
using System;

namespace NSeed.Discovery
{
    internal interface IMetaInfoPool<TImplementation, TMetaInfo>
        where TImplementation : class
        where TMetaInfo : BaseMetaInfo
    {
        TMetaInfo GetOrAdd(TImplementation implementation, Func<TImplementation, TMetaInfo> metaInfoFactory);
    }
}