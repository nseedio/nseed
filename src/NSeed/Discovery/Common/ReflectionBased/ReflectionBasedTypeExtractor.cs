using NSeed.MetaInfo;
using System;

namespace NSeed.Discovery.Common.ReflectionBased
{
    internal class ReflectionBasedTypeExtractor : ITypeExtractor<Type>
    {
        Type IExtractor<Type, Type>.ExtractFrom(Type implementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(implementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return implementation;
        }
    }
}
