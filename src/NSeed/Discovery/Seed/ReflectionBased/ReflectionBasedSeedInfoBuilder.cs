using System;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal abstract class ReflectionBasedSeedInfoBuilder : BaseSeedInfoBuilder<Type>
    {
        internal ReflectionBasedSeedInfoBuilder()
            : base(new ReflectionBasedSeedTypeExtractor(),
                   new ReflectionBasedSeedFullNameExtractor(),
                   new ReflectionBasedSeedFriendlyNameExtractor())
        {
        }
    }
}