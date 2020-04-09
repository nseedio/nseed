using NSeed;
using System.Threading.Tasks;

namespace SeedsProject
{
    public class SeedsProjectSeedBucket : SeedBucket
    {
        internal static async Task<int> Main(string[] args)
            => await Handle<SeedsProjectSeedBucket>(args);
    }
}