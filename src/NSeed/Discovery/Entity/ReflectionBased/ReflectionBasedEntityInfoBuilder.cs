using System;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal abstract class ReflectionBasedEntityInfoBuilder : BaseEntityInfoBuilder<Type>
    {
        internal ReflectionBasedEntityInfoBuilder()
            : base(new ReflectionBasedEntityTypeExtractor(),
                   new ReflectionBasedEntityFullNameExtractor())
        {
        }
    }
}