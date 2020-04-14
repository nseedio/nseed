using System;
using System.Reflection;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedAlwaysRequiredExtractor : ISeedAlwaysRequiredExtractor<Type>
    {
        bool IExtractor<Type, bool>.ExtractFrom(Type implementation) => implementation.GetCustomAttribute<AlwaysRequiredAttribute>() != null;
    }
}
