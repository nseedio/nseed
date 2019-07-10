using System;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Yield.ReflectionBased
{
    internal class ReflectionBasedYieldFullNameExtractor : IYieldFullNameExtractor<Type>
    {
        string IExtractor<Type, string>.ExtractFrom(Type yieldImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(yieldImplementation.IsYieldType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return yieldImplementation.FullName;
        }
    }
}