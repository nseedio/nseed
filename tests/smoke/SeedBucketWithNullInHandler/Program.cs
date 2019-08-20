using NSeed;
using System.Threading.Tasks;

namespace SeedBucketWithNullInHandler
{
    internal class SeedBucketWithNullInHandler : SeedBucket
    {
        internal static async Task<int> Main()
            => await Handle<SeedBucketWithNullInHandler>(null);
    }
}
