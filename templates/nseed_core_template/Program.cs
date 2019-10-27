using NSeed;
using System.Threading.Tasks;

namespace nseed_core_template
{
    internal class nseed_core_templateSeedBucket : SeedBucket
    {
        internal static async Task<int> Main(string[] args)
            => await Handle<nseed_core_templateSeedBucket>(args);
    }
}