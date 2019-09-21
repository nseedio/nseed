using System;

namespace NSeed.Discovery.Common.ReflectionBased
{
    internal class ReflectionBasedFullNameExtractor : IFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type implementation)
        {
            System.Diagnostics.Debug.Assert(implementation != null);

            return implementation.FullName;
        }
    }
}
