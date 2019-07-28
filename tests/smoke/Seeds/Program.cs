using NSeed;
using System.Threading.Tasks;

namespace Seeds
{
    internal class Seeds : SeedBucket
    {
        internal static async Task<int> Main(string[] args)
            => await Handle<Seeds>(args);
    }
}
