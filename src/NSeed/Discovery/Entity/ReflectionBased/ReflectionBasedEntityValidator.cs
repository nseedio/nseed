using NSeed.Discovery.ErrorMessages;
using NSeed.Extensions;
using NSeed.MetaInfo;
using System;

namespace NSeed.Discovery.Entity.ReflectionBased
{
    internal sealed class ReflectionBasedEntityValidator : BaseEntityValidator<Type>
    {
        protected override Error? EntityMustNotBeNSeedType(Type implementation)
        {
            return implementation.Assembly == typeof(ISeed).Assembly
                ? Errors.Entity.EntityMustNotBeNSeedType(implementation.FullName)
                : null;
        }

        protected override Error? EntityMustNotBeStaticType(Type implementation)
        {
            return implementation.IsAbstract && implementation.IsSealed
                ? Errors.Entity.EntityMustNotBeStaticType(implementation.FullName)
                : null;
        }

        protected override Error? EntityMustNotBeSeed(Type implementation)
        {
            return implementation.IsSeedType()
                ? Errors.Entity.EntityMustNotBeSeed(implementation.FullName)
                : null;
        }

        protected override Error? EntityMustNotBeScenario(Type implementation)
        {
            return implementation.IsScenarioType()
                ? Errors.Entity.EntityMustNotBeScenario(implementation.FullName)
                : null;
        }
    }
}
