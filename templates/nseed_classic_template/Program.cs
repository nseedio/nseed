using NSeed;
using System.Threading.Tasks;

namespace nseed_classic_template
{
    internal class nseed_classic_templateSeedBucket : SeedBucket
    {
        internal static async Task<int> Main(string[] args)
            => await Handle<nseed_classic_templateSeedBucket>(args);
    }
}