using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.MetaInfo
{
    internal class DistinctErrorCollectorAndProvider : IErrorCollector, IErrorProvider
    {
        private readonly List<Error> errors = new List<Error>();

        void IErrorCollector.Collect(Error error)
        {
            System.Diagnostics.Debug.Assert(error != null);

            if (errors.Any(existingError => existingError.Code == error.Code)) return;

            errors.Add(error);
        }

        bool IErrorCollector.Collect(Action<IErrorCollector> collecting)
        {
            int errorsBefore = errors.Count;

            collecting(this);

            return errorsBefore != errors.Count;
        }

        public IReadOnlyCollection<Error> GetErrors()
        {
            return errors;
        }
    }
}
