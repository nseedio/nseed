using NSeed.Discovery.Common.ReflectionBased;
using System;

namespace NSeed.Discovery.Yield.ReflectionBased
{
    internal class ReflectionBasedProvidedYieldInfoBuilder : BaseProvidedYieldInfoBuilder<Type>
    {
        public ReflectionBasedProvidedYieldInfoBuilder()
            : base(new ReflectionBasedTypeExtractor(),
                   new ReflectionBasedYieldFullNameExtractor())
        {
        }
    }
}