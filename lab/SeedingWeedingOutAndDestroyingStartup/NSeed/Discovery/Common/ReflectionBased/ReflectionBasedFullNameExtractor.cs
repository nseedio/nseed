using System;

namespace NSeed.Discovery.Common.ReflectionBased
{
    internal class ReflectionBasedFullNameExtractor : IFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type implementation)
        {
            return implementation.FullName;
        }
    }
}
