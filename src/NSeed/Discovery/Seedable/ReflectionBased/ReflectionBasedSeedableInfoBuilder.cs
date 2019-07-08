using System;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedableInfoBuilder : BaseSeedableInfoBuilder<Type, Type>
    {
        public ReflectionBasedSeedableInfoBuilder()
            : base(new ReflectionBasedSeedableTypeExtractor(),
                   new ReflectionBasedSeedableFullNameExtractor(),
                   new ReflectionBasedSeedableFriendlyNameExtractor(),
                   new ReflectionBasedSeedableDescriptionExtractor(),
                   new ReflectionBasedSeedEntitiesExtractor(),
                   builder => new ReflectionBasedExplicitlyRequiredSeedablesExtractor(builder),
                   new ReflectionBasedSeedableInfoPool())
        {
        }

        protected internal override bool IsSeedImplemenation(Type implementation)
        {
            return implementation.IsSeedType();
        }
    }
}