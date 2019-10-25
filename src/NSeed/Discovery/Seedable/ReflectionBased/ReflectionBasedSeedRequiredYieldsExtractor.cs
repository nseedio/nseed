using NSeed.Discovery.Yield;
using NSeed.Discovery.Yield.ReflectionBased;
using NSeed.Extensions;
using NSeed.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSeed.Discovery.Seedable
{
    internal class ReflectionBasedSeedRequiredYieldsExtractor : ISeedRequiredYieldsExtractor<Type>
    {
        private readonly IRequiredYieldAccessPropertyInSeedDiscoverer<Type, PropertyInfo> requiredYieldAccessPropertyInSeedDiscoverer;
        private readonly ISeedableInfoBuilder<Type> seedableBuilder;

        internal ReflectionBasedSeedRequiredYieldsExtractor(ISeedableInfoBuilder<Type> seedableBuilder)
        {
            requiredYieldAccessPropertyInSeedDiscoverer = new ReflectionBasedRequiredYieldAccessPropertyInSeedDiscoverer();
            this.seedableBuilder = seedableBuilder;
        }

        IReadOnlyCollection<RequiredYieldInfo> IExtractor<Type, IReadOnlyCollection<RequiredYieldInfo>>.ExtractFrom(Type seedImplementation)
        {
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            // CS8600:
            // CS8604:
            // Everything is fine here.
            // The BuildFrom can return null that will be filtered out in the "Where(...)".
            // Compiler is not able to figure this out.
            return requiredYieldAccessPropertyInSeedDiscoverer.DiscoverIn(seedImplementation)
                                .DiscoveredItems
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                                .Select(property => (property, yieldingSeed: (SeedInfo)seedableBuilder.BuildFrom(property.PropertyType.DeclaringType)))
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                                .Where(propertyAndYieldingSeed => propertyAndYieldingSeed.yieldingSeed != null)

                                // TODO-IG: This extractor is at the same time an extractor and a builder.
                                //          I obviously got a bit tired of all this strict single responsibility principle stuff.
                                //          Get back to track and refactor ASAP.
                                .Select(propertyAndYieldingSeed => new RequiredYieldInfo
                                (
#pragma warning disable CS8604 // Possible null reference argument.
                                    propertyAndYieldingSeed.yieldingSeed,
#pragma warning restore CS8604 // Possible null reference argument.
                                    propertyAndYieldingSeed.property,
                                    propertyAndYieldingSeed.property.Name,
                                    Array.Empty<Error>()
                                ))
                                .ToArray();
        }
    }
}
