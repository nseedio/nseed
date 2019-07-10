using System;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Yield.ReflectionBased
{
    internal class ReflectionBasedYieldTypeExtractor : IYieldTypeExtractor<Type>
    {
        Type IExtractor<Type, Type>.ExtractFrom(Type yieldImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(yieldImplementation.IsYieldType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return yieldImplementation;
        }
    }
}