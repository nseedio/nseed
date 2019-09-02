using NSeed;
using System;
using System.Threading.Tasks;

namespace nseed_core_template
{
    /// <summary>
    /// Your domain model 
    /// </summary>
    internal class SeedModel { }

    internal class YourFirstSeed : ISeed<SeedModel>
    {
        public Task<bool> HasAlreadyYielded()
        {
            throw new NotImplementedException();
        }

        public Task Seed()
        {
            throw new NotImplementedException();
        }
    }

    internal class Yield : YieldOf<YourFirstSeed> { }
}