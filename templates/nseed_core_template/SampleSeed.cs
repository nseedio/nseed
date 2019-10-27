using NSeed;
using System;
using System.Threading.Tasks;

namespace nseed_core_template
{
    // TODO: To learn how to write seeds see TODO-URL.
    internal sealed class SampleSeed : ISeed
    {
        public Task Seed()
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasAlreadyYielded()
        {
            throw new NotImplementedException();
        }
    }

    internal class Yield : YieldOf<SampleSeed>
    {
        // TODO: Use the Yield class to provide the yield of this seed to other seeds.
        //       To learn how to use the Yield class see TODO-URL.
    }
}