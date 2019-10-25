using System;
using System.Linq;

namespace NSeed.Discovery.Common.ReflectionBased
{
    internal class ReflectionBasedDescriptionExtractor : IDescriptionExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type implementation)
        {
            var description = implementation
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault()?
                .Description;

            return description ?? string.Empty;
        }
    }
}
