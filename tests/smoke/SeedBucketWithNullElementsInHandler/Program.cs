using NSeed;
using System.Threading.Tasks;

namespace SeedBucketWithNullElementsInHandler
{
    internal class SeedBucketWithNullElementsInHandler : SeedBucket
    {
        internal static async Task<int> Main()
            => await Handle<SeedBucketWithNullElementsInHandler>(new[] { "-nc", null, "-v" });
    }
}
