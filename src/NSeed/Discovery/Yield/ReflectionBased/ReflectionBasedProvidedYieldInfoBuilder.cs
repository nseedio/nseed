using System;

namespace NSeed.Discovery.Yield.ReflectionBased
{
    internal class ReflectionBasedProvidedYieldInfoBuilder : BaseProvidedYieldInfoBuilder<Type>
    {
        public ReflectionBasedProvidedYieldInfoBuilder()
            : base(new ReflectionBasedYieldTypeExtractor(),
                   new ReflectionBasedYieldFullNameExtractor())
        {
        }
    }
}