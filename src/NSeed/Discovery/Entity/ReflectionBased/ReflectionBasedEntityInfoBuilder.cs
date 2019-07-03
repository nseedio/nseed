using System;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal class ReflectionBasedEntityInfoBuilder : BaseEntityInfoBuilder<Type>
    {
        public ReflectionBasedEntityInfoBuilder()
            : base(new ReflectionBasedEntityTypeExtractor(),
                   new ReflectionBasedEntityFullNameExtractor(),
                   new ReflectionBasedEntityInfoPool())
        {
        }
    }
}