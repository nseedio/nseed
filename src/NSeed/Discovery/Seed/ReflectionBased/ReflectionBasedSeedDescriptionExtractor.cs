using System;
using NSeed.Guards;
using System.Linq;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedDescriptionExtractor : ISeedDescriptionExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type seedImplementation, IErrorCollector errorCollector)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            var description = seedImplementation
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault()?
                .Description;

            return description ?? string.Empty;

            // TODO-IG: Collect errors.
        }
    }
}