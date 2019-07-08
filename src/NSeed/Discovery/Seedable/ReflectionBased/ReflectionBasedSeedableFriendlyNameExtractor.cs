using System;
using System.Linq;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedableFriendlyNameExtractor : ISeedableFriendlyNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type seedableImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedableImplementation.IsSeedableType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            var friendlyNameAttribute = seedableImplementation
                .GetCustomAttributes(typeof(FriendlyNameAttribute), false)
                .Cast<FriendlyNameAttribute>()
                .FirstOrDefault();

            if (friendlyNameAttribute == null)
                return seedableImplementation.Name.Humanize();

            var friendlyName = friendlyNameAttribute.FriendlyName;

            bool hasErrors = errorCollector.Collect(collector =>
            {
                if (friendlyName == null)
                    collector.Collect(Errors.Seedable.FriendlyName.MustNotBeNull);
                else if (string.IsNullOrEmpty(friendlyName))
                    collector.Collect(Errors.Seedable.FriendlyName.MustNotBeEmptyString);
                else if (string.IsNullOrWhiteSpace(friendlyName))
                    collector.Collect(Errors.Seedable.FriendlyName.MustNotBeWhitespace);
            });

            return hasErrors
                ? seedableImplementation.Name.Humanize()
                : friendlyName;
        }
    }
}