using NSeed.MetaInfo;
using System.Collections.Generic;

namespace NSeed.Discovery
{
    internal interface IValidator<TImplementation>
        where TImplementation : class
    {
        IReadOnlyCollection<Error> Validate(TImplementation implementation);
    }
}
