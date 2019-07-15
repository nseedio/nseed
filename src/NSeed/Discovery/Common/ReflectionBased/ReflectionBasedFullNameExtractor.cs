using System;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Common.ReflectionBased
{
    internal class ReflectionBasedFullNameExtractor : IFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type implementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(implementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return implementation.FullName;
        }
    }
}