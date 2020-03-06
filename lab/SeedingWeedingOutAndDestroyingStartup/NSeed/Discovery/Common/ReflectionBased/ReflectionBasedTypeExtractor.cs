using System;

namespace NSeed.Discovery.Common.ReflectionBased
{
    internal class ReflectionBasedTypeExtractor : ITypeExtractor<Type>
    {
        Type IExtractor<Type, Type?>.ExtractFrom(Type implementation)
        {
            return implementation;
        }
    }
}
