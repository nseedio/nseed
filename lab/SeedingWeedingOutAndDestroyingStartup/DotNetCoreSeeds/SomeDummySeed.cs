using GettingThingsDone.Contracts.Model;
using GettingThingsDone.Contracts.Model.SomeSubmodel;
using NSeed;
using System.Threading.Tasks;

namespace DotNetCoreSeeds
{
    public class SomeDummySeed : ISeed<DummyEntity, Project>
    {
        public Task<bool> HasAlreadyYielded()
        {
            return Task.FromResult(true);
        }

        public Task Seed()
        {
            return Task.CompletedTask;
        }
    }
}
