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
            System.Diagnostics.Debug.Assert(seedImplementation.IsSeedType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            var friendlyNameAttribute = seedImplementation
                .GetCustomAttributes(typeof(FriendlyNameAttribute), false)
                .Cast<FriendlyNameAttribute>()
                .FirstOrDefault();

            if (friendlyNameAttribute == null)
                return seedImplementation.Name.Humanize();

            var friendlyName = friendlyNameAttribute.FriendlyName;

            bool hasErrors = errorCollector.Collect(collector =>
            {
                if (friendlyName == null)
                    collector.Collect(Errors.Seed.FriendlyName.MustNotBeNull);
                else if (string.IsNullOrEmpty(friendlyName))
                    collector.Collect(Errors.Seed.FriendlyName.MustNotBeEmptyString);
                else if (string.IsNullOrWhiteSpace(friendlyName))
                    collector.Collect(Errors.Seed.FriendlyName.MustNotBeWhitespace);
            });

            return hasErrors
                ? seedImplementation.Name.Humanize()
                : friendlyName;
        }
    }
}