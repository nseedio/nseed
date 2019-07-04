using System.Collections.Generic;
using System.Linq;

namespace NSeed.MetaInfo
{
    internal class DistinctErrorCollectorAndProvider : IErrorCollector, IErrorProvider
    {
        private readonly List<Error> errors = new List<Error>();

        void IErrorCollector.Add(Error error)
        {
            System.Diagnostics.Debug.Assert(error != null);

            if (errors.Any(existingError => existingError.Code == error.Code)) return;

            errors.Add(error);
        }

        IReadOnlyCollection<Error> IErrorProvider.GetErrors()
        {
            return errors;
        }
    }
}
