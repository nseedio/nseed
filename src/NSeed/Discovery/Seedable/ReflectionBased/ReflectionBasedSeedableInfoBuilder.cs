using NSeed.Discovery.Common.ReflectionBased;
using NSeed.Extensions;
using System;

namespace NSeed.Discovery.Seedable.ReflectionBased
{
    internal class ReflectionBasedSeedableInfoBuilder : BaseSeedableInfoBuilder<Type, Type>
    {
        public ReflectionBasedSeedableInfoBuilder()
            : base(new ReflectionBasedTypeExtractor(),
                   new ReflectionBasedFullNameExtractor(),
                   new ReflectionBasedSeedableFriendlyNameExtractor(),
                   new ReflectionBasedSeedableDescriptionExtractor(),
                   new ReflectionBasedSeedEntitiesExtractor(),
                   new ReflectionBasedSeedProvidedYieldExtractor(),
                   builder => new ReflectionBasedExplicitlyRequiredSeedablesExtractor(builder),
                   builder => new ReflectionBasedSeedRequiredYieldsExtractor(builder),
                   new ReflectionBasedSeedableInfoPool())
        {
        }

        protected internal override bool IsSeedImplemenation(Type implementation)
        {
            return implementation.IsSeedType();
        }
    }
}