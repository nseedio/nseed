using System;
using NSeed.Guards;
using System.Linq;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedDescriptionExtractor : ISeedDescriptionExtractor<Type>
    {
        string ISeedDescriptionExtractor<Type>.ExtractFrom(Type seedImplementation)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            var description = seedImplementation
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault()?
                .Description;

            return description ?? string.Empty;
        }
    }
}