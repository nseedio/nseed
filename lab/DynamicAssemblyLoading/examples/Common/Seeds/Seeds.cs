using NSeed.Seeding;
using System.Threading.Tasks;

namespace SmokeTests.Seeds
{
    class Seeds : SeedBucket
    {
        static async Task Main(string[] args) => await Seed<Seeds>(args);
    }
}