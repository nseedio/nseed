using System.Threading.Tasks;
using NSeed;

namespace EmptySeedBucket
{
    internal class EmptySeedBucket : SeedBucket
    {
        internal static async Task<int> Main(string[] args)
            => await Handle<EmptySeedBucket>(args);
    }
}
