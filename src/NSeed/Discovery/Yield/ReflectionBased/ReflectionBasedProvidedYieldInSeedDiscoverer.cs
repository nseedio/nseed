using NSeed.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace NSeed.Discovery.Yield.ReflectionBased
{
    internal class ReflectionBasedProvidedYieldInSeedDiscoverer : IProvidedYieldInSeedDiscoverer<Type, Type>
    {
        Discovery<Type> IDiscoverer<Type, Type>.DiscoverIn(Type source)
        {
            System.Diagnostics.Debug.Assert(source.IsSeedType());

            var yieldTypes = source.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                .Where(type => type.IsYieldType())
                .ToArray();

            if (yieldTypes.Length > 1) yieldTypes = Array.Empty<Type>();

            return new Discovery<Type>(yieldTypes);
        }
    }
}
