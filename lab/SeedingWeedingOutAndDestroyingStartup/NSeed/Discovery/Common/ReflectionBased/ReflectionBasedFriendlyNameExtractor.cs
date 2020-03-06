using NSeed.Extensions;
using System;
using System.Linq;

namespace NSeed.Discovery.Common.ReflectionBased
{
    internal class ReflectionBasedFriendlyNameExtractor : IFriendlyNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type implementation)
        {
            var friendlyNameAttribute = implementation
                .GetCustomAttributes(typeof(FriendlyNameAttribute), false)
                .Cast<FriendlyNameAttribute>()
                .FirstOrDefault();

            if (friendlyNameAttribute == null)
                return implementation.Name.Humanize();

            var friendlyName = friendlyNameAttribute.FriendlyName;

            return string.IsNullOrWhiteSpace(friendlyName)
                ? implementation.Name.Humanize()
                : friendlyName;
        }
    }
}
