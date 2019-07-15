using System;
using System.Linq;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Common.ReflectionBased
{
    internal class ReflectionBasedFriendlyNameExtractor : IFriendlyNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type implementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(implementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            var friendlyNameAttribute = implementation
                .GetCustomAttributes(typeof(FriendlyNameAttribute), false)
                .Cast<FriendlyNameAttribute>()
                .FirstOrDefault();

            if (friendlyNameAttribute == null)
                return implementation.Name.Humanize();

            var friendlyName = friendlyNameAttribute.FriendlyName;

            bool hasErrors = errorCollector.Collect(collector =>
            {
                if (friendlyName == null)
                    collector.Collect(Errors.FriendlyName.MustNotBeNull);
                else if (string.IsNullOrEmpty(friendlyName))
                    collector.Collect(Errors.FriendlyName.MustNotBeEmptyString);
                else if (string.IsNullOrWhiteSpace(friendlyName))
                    collector.Collect(Errors.FriendlyName.MustNotBeWhitespace);
            });

            return hasErrors
                ? implementation.Name.Humanize()
                : friendlyName;
        }
    }
}