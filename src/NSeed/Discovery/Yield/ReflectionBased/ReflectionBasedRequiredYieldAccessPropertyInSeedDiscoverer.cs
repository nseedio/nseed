using NSeed.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace NSeed.Discovery.Yield.ReflectionBased
{
    internal class ReflectionBasedRequiredYieldAccessPropertyInSeedDiscoverer : IRequiredYieldAccessPropertyInSeedDiscoverer<Type, PropertyInfo>
    {
        Discovery<PropertyInfo> IDiscoverer<Type, PropertyInfo>.DiscoverIn(Type source)
        {
            System.Diagnostics.Debug.Assert(source.IsSeedType());

            // TODO-CHECK: Must not be static. Must be autoproperty.

            var propertyInfos = source
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                .Where(property => property.PropertyType.IsYieldType() && !property.PropertyType.IsYieldTypeOfSeed(source))
                .ToArray();

            return new Discovery<PropertyInfo>(propertyInfos);
        }
    }
}
