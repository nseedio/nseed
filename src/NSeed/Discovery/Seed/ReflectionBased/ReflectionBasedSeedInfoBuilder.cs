using System;

namespace NSeed.Discovery.Seed.ReflectionBased
{
    internal class ReflectionBasedSeedInfoBuilder : BaseSeedInfoBuilder<Type, Type>
    {
        public ReflectionBasedSeedInfoBuilder()
            : base(new ReflectionBasedSeedTypeExtractor(),
                   new ReflectionBasedSeedFullNameExtractor(),
                   new ReflectionBasedSeedFriendlyNameExtractor(),
                   new ReflectionBasedSeedDescriptionExtractor(),
                   new ReflectionBasedSeedEntitiesExtractor())
        {
        }
    }
}