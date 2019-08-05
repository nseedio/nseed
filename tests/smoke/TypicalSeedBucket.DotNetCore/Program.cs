using NSeed;
using System.Threading.Tasks;

namespace Seeds
{
    internal class TypicalSeedBucket : SeedBucket
    {
        internal static async Task<int> Main(string[] args)
            => await Handle<TypicalSeedBucket>(args);
    }
}
