using NSeed;
using System.Threading.Tasks;

namespace SeedBucketWithErrors.DotNetCore
{
    internal class SeedBucketWithErrors : SeedBucket
    {
        internal static async Task<int> Main(string[] args)
            => await Handle<SeedBucketWithErrors>(args);
    }
}
