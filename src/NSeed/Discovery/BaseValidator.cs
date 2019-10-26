using NSeed.MetaInfo;
using System;
using System.Collections.Generic;

namespace NSeed.Discovery
{
    internal abstract class BaseValidator<TImplementation> : IValidator<TImplementation>
        where TImplementation : class
    {
        // No reflection based search for validator methods.
        // Let the derived classes list their validators explicitly.
        protected Func<TImplementation, Error?>[] Validators { get; set; }

        protected BaseValidator()
        {
            Validators = Array.Empty<Func<TImplementation, Error?>>();
        }

        public IReadOnlyCollection<Error> Validate(TImplementation implementation)
        {
            var result = new List<Error>(Validators.Length);

            foreach (var validator in Validators)
            {
                var error = validator(implementation);
                if (error != null) result.Add(error);
            }

            return result;
        }
    }
}
