using System;
using System.Linq;
using NSeed.MetaInfo;
using System.Collections.Generic;
using NSeed.Discovery.Yield;
using System.Reflection;
using NSeed.Discovery.Yield.ReflectionBased;
using NSeed.Extensions;

namespace NSeed.Discovery.Seedable
{
    internal class ReflectionBasedSeedRequiredYieldsExtractor : ISeedRequiredYieldsExtractor<Type>
    {
        private readonly IRequiredYieldAccessPropertyInSeedDiscoverer<Type, PropertyInfo> requiredYieldAccessPropertyInSeedDiscoverer;
        private readonly ISeedableInfoBuilder<Type> seedableBuilder;

        internal ReflectionBasedSeedRequiredYieldsExtractor(ISeedableInfoBuilder<Type> seedableBuilder)
        {
            System.Diagnostics.Debug.Assert(seedableBuilder != null);

            requiredYieldAccessPropertyInSeedDiscoverer = new ReflectionBasedRequiredYieldAccessPropertyInSeedDiscoverer();
            this.seedableBuilder = seedableBuilder;
        }

        IReadOnlyCollection<RequiredYieldInfo> IExtractor<Type, IReadOnlyCollection<RequiredYieldInfo>>.ExtractFrom(Type seedImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return requiredYieldAccessPropertyInSeedDiscoverer.DiscoverIn(seedImplementation)
                                .DiscoveredItems
                                .Select(property => (property, yieldingSeed: (SeedInfo)seedableBuilder.BuildFrom(property.PropertyType.DeclaringType)))
                                .Where(propertyAndYieldingSeed => propertyAndYieldingSeed.yieldingSeed != null)
                                // TODO-IG: This extractor is at the same time an extractor and a builder.
                                //          I obviously got a bit tired of all this strict single responsibility principle stuff.
                                //          Get back to track and refactor ASAP.
                                .Select(propertyAndYieldingSeed => new RequiredYieldInfo
                                (
                                    propertyAndYieldingSeed.yieldingSeed,
                                    propertyAndYieldingSeed.property,
                                    propertyAndYieldingSeed.property.Name
                                ))
                                .ToArray();
        }
    }
}