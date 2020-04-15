using NSeed;
using System.Threading.Tasks;

namespace DotNetCoreSeeds
{
    public class SomeDummySeedWithoutEntities : ISeed
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
