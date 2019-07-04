using System;
using NSeed.Guards;
using System.Linq;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedFriendlyNameExtractor : ISeedFriendlyNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type seedImplementation, IErrorCollector errorCollector)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            var friendlyName = seedImplementation
                .GetCustomAttributes(typeof(FriendlyNameAttribute), false)
                .Cast<FriendlyNameAttribute>()
                .FirstOrDefault()?
                .FriendlyName;

            return friendlyName ?? seedImplementation.Name.Humanize();

            // TODO-IG: Collect errors.
        }
    }
}