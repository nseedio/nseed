using System;
using System.Collections.Concurrent;

namespace NSeed.Discovery
{
    internal class BaseMetaInfoPool<TImplementation, TMetaInfo> : IMetaInfoPool<TImplementation, TMetaInfo>
        where TImplementation : class
        where TMetaInfo : MetaInfo.MetaInfo
    {
        private readonly ConcurrentDictionary<TImplementation, TMetaInfo> pool = new ConcurrentDictionary<TImplementation, TMetaInfo>();

        TMetaInfo IMetaInfoPool<TImplementation, TMetaInfo>.GetOrAdd(TImplementation implementation, Func<TImplementation, TMetaInfo> metaInfoFactory)
        {
            System.Diagnostics.Debug.Assert(implementation != null);
            System.Diagnostics.Debug.Assert(metaInfoFactory != null);

            return pool.GetOrAdd(implementation, metaInfoFactory);
        }
    }
}
