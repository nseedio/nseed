using NSeed;
using System;
using System.Threading.Tasks;

namespace Seeds.Seeding.People
{
    internal sealed class FirstValidSeed : ISeed
    {
        public Task<bool> HasAlreadyYielded() => throw new NotImplementedException();

        public Task Seed() => throw new NotImplementedException();
    }

    internal sealed class SecondValidSeed : ISeed
    {
        public Task<bool> HasAlreadyYielded() => throw new NotImplementedException();

        public Task Seed() => throw new NotImplementedException();
    }

    internal sealed class SeedWithInvalidEntities : ISeed<ISeed, SeedWithInvalidEntities, SomeScenario>
    {
        public Task<bool> HasAlreadyYielded() => throw new NotImplementedException();

        public Task Seed() => throw new NotImplementedException();
    }

    internal class SomeScenario : IScenario { }
}
