using NSeed.Extensions;
using System;
using System.Linq;

namespace NSeed.Discovery.SeedBucket.ReflectionBased
{
    internal class ReflectionBasedContainedSeedablesDiscoverer : IContainedSeedablesDiscoverer<Type, Type>
    {
        Discovery<Type> IDiscoverer<Type, Type>.DiscoverIn(Type source)
        {
            System.Diagnostics.Debug.Assert(source.IsSeedBucketType());

            var containedSeedables = source
                .Assembly
                .GetTypes()
                .Where(type => type.IsSeedableType())
                .ToArray();

            return new Discovery<Type>(containedSeedables);
        }
    }
}