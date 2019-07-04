using System.Collections.Generic;

namespace NSeed.MetaInfo
{
    internal interface IErrorProvider
    {
        IReadOnlyCollection<Error> GetErrors();
    }
}
