using System;
using System.Threading.Tasks;

namespace NSeed.Tests.Unit.Discovery.Seed
{
    internal abstract class BaseTestSeed : ISeed
    {
        public Task<bool> HasAlreadyYielded()
        {
            throw new NotImplementedException();
        }

        public Task Seed()
        {
            throw new NotImplementedException();
        }

        public Task WeedOut()
        {
            throw new NotImplementedException();
        }
    }
}