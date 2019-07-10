using System;
using System.Threading.Tasks;

namespace NSeed.Tests.Unit.Discovery.Seedable
{
    internal abstract class BaseTestSeed : ISeed
    {
        public Task<bool> HasAlreadyYielded() => throw new NotImplementedException();
        public Task Seed() => throw new NotImplementedException();
    }

    internal abstract class BaseTestSeed<TEntity> : ISeed<TEntity>
    {
        public Task<bool> HasAlreadyYielded() => throw new NotImplementedException();
        public Task Seed() => throw new NotImplementedException();
    }

    internal abstract class BaseTestSeed<TEntity1, TEntity2> : ISeed<TEntity1, TEntity2>
    {
        public Task<bool> HasAlreadyYielded() => throw new NotImplementedException();
        public Task Seed() => throw new NotImplementedException();
    }

    internal abstract class BaseTestSeed<TEntity1, TEntity2, TEntity3> : ISeed<TEntity1, TEntity2, TEntity3>
    {
        public Task<bool> HasAlreadyYielded() => throw new NotImplementedException();
        public Task Seed() => throw new NotImplementedException();
    }
}