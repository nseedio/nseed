using NSeed;
using System.Threading.Tasks;

namespace Seeds
{
    public class Class1 : SeedBucket
    {
        internal static async Task<int> Main(string[] args)
            => await Handle<Class1>(args);
    }
}
