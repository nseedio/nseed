using NSeed.MetaInfo;
using System;

namespace NSeed.Discovery.Entity
{
    internal abstract class BaseEntityValidator<TImplementation> : BaseValidator<TImplementation>
        where TImplementation : class
    {
        protected BaseEntityValidator()
        {
            Validators = new Func<TImplementation, Error?>[]
            {
                EntityMustNotBeNSeedType,
                EntityMustNotBeStaticType,
                EntityMustNotBeSeed,
                EntityMustNotBeScenario
            };
        }

        protected abstract Error? EntityMustNotBeNSeedType(TImplementation implementation);

        protected abstract Error? EntityMustNotBeStaticType(TImplementation implementation);

        protected abstract Error? EntityMustNotBeSeed(TImplementation implementation);

        protected abstract Error? EntityMustNotBeScenario(TImplementation implementation);
    }
}
