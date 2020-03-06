using NSeed.Extensions;
using System;
using System.Linq;

namespace NSeed.Discovery.SeedBucketStartup.ReflectionBased
{
    internal class ReflectionBasedSeedBucketStartupInSeedBucketDiscoverer : ISeedBucketStartupInSeedBucketDiscoverer<Type, Type>
    {
        Discovery<Type> IDiscoverer<Type, Type>.DiscoverIn(Type source)
        {
            System.Diagnostics.Debug.Assert(source.IsSeedBucketType());

            var seedBucketStartups = source
                .Assembly
                .GetTypes()
                .Where(type => type.IsSeedBucketStartupType())
                .ToArray();

            return new Discovery<Type>(seedBucketStartups);
        }
    }
}
