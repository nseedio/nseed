using NSeed.Extensions;
using System;
using System.Linq;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityInSeedDiscoverer : IEntityInSeedDiscoverer<Type, Type>
    {
        Discovery<Type> IDiscoverer<Type, Type>.DiscoverIn(Type source)
        {
            System.Diagnostics.Debug.Assert(source.IsSeedType());

            // TODO-ERROR: Entity defined more then once.
            // TODO-ERROR: Seed implements different ISeeds.
            // TODO-ERROR: Entity type is generic parameter.

            var entityTypes = source.GetInterfaces()
                .Where(@interface =>
                    @interface.IsConstructedGenericType &&
                    @interface.GetGenericTypeDefinition().IsSeedInterfaceTypeWithEntities())
                .SelectMany(seedInterfaceWithEntities => seedInterfaceWithEntities.GetGenericArguments())
                .Distinct()
                .ToArray();

            return new Discovery<Type>(entityTypes);
        }
    }
}