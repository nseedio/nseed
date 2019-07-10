using System;
using System.Linq;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedableDescriptionExtractor : ISeedableDescriptionExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type seedableImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedableImplementation.IsSeedableType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            var description = seedableImplementation
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault()?
                .Description;

            return description ?? string.Empty;
        }
    }
}