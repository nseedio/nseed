using NSeed.Extensions;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace NSeed.Discovery.SeedBucket.ReflectionBased
{
    internal class ReflectionBasedSeedBucketOfSeedableDiscoverer : ISeedBucketOfSeedableDiscoverer<Type, Type>
    {
        private readonly ConcurrentDictionary<Assembly, Discovery<Type>> seedBucketsInAssembly = new ConcurrentDictionary<Assembly, Discovery<Type>>();

        Discovery<Type> IDiscoverer<Type, Type>.DiscoverIn(Type source)
        {
            System.Diagnostics.Debug.Assert(source.IsSeedableType());

            return seedBucketsInAssembly.GetOrAdd(source.Assembly, GetSeedBucketTypesInAssembly);
        }

        private static Discovery<Type> GetSeedBucketTypesInAssembly(Assembly assembly)
        {
            var seedBuckets = assembly
                .GetTypes()
                .Where(type => type.IsSeedBucketType())
                .ToArray();

            seedBuckets = seedBuckets.Length == 1
                ? seedBuckets
                : Array.Empty<Type>();

            return new Discovery<Type>(seedBuckets);
        }
    }
}
