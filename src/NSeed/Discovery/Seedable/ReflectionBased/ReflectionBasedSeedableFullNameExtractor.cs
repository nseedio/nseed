using System;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedableFullNameExtractor : ISeedableFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type seedableImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedableImplementation.IsSeedableType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return seedableImplementation.FullName;
        }
    }
}