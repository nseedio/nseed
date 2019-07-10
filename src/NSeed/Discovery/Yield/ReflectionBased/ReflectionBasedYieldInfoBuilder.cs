using System;

namespace NSeed.Discovery.Yield.ReflectionBased
{
    internal class ReflectionBasedYieldInfoBuilder : BaseYieldInfoBuilder<Type>
    {
        public ReflectionBasedYieldInfoBuilder()
            : base(new ReflectionBasedYieldTypeExtractor(),
                   new ReflectionBasedYieldFullNameExtractor())
        {
        }
    }
}