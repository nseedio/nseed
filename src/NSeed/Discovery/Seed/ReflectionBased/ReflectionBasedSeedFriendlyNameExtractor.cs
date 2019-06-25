using System;
using Light.GuardClauses;
using NSeed.Guards;
using System.Linq;
using Humanizer;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedFriendlyNameExtractor : ISeedFriendlyNameExtractor<Type>
    {
        string ISeedFriendlyNameExtractor<Type>.ExtractFrom(Type seedImplementation)
        {
            seedImplementation.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(seedImplementation)));
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());

            var friendlyName = seedImplementation
                .GetCustomAttributes(typeof(FriendlyNameAttribute), false)
                .Cast<FriendlyNameAttribute>()
                .FirstOrDefault()?
                .FriendlyName;

            return friendlyName ?? seedImplementation.Name.Humanize();
        }
    }
}