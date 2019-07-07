using System;
using System.Linq;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedExplicitlyRequiredSeedableDiscoverer : IExplicitlyRequiredSeedableDiscoverer<Type, Type>
    {
        Discovery<Type> IDiscoverer<Type, Type>.DiscoverIn(Type source)
        {
            System.Diagnostics.Debug.Assert(source.IsSeedableType());

            // TODO-ERROR: Seedable defined more then once.
            // TODO-ERROR: Seedable is null.
            // TODO-ERROR: Seedable is neither ISeed nor IScenario.

            var requiredSeedables = source
                .GetCustomAttributes(typeof(RequiresAttribute), true)
                .Cast<RequiresAttribute>()
                .Where(required =>
                    required.SeedableType != null &&
                    required.SeedableType.IsSeedableType()
                )
                .Select(required => required.SeedableType)
                .Distinct()
                .ToArray();

            return new Discovery<Type>(requiredSeedables);
        }
    }
}