using NSeed.Abstractions;
using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Seeding;
using System;
using System.Threading.Tasks;

namespace NSeed.Sdk
{
    public static class Get
    {
        public static async Task<TYieldOf> Yield<TYieldOf>()
        {
            // TODO: Chek type is yield.
            return (TYieldOf)(await SeedAndGetYields(null, null, null, typeof(TYieldOf)))[0];
        }

        public static async Task<TYieldOf> Yield<TYieldOf>(Type seedBucketStartupType)
        {
            // TODO: Chek type is yield.
            return (TYieldOf)(await SeedAndGetYields(seedBucketStartupType, null, null, typeof(TYieldOf)))[0];
        }

        public static async Task<TYieldOf> Yield<TYieldOf>(SeedBucketStartup seedBucketStartup, IOutputSink? outputSink = null)
        {
            // TODO: Chek type is yield.
            return (TYieldOf)(await SeedAndGetYields(null, seedBucketStartup, outputSink, typeof(TYieldOf)))[0];
        }

        public static async Task<(TYieldOf1, TYieldOf2)> Yields<TYieldOf1, TYieldOf2>()
        {
            var yields = await SeedAndGetYields(null, null, null, typeof(TYieldOf1), typeof(TYieldOf2));
            return ((TYieldOf1)yields[0], (TYieldOf2)yields[1]);
        }

        public static async Task<(TYieldOf1, TYieldOf2)> Yields<TYieldOf1, TYieldOf2>(Type seedBucketStartupType)
        {
            var yields = await SeedAndGetYields(seedBucketStartupType, null, null, typeof(TYieldOf1), typeof(TYieldOf2));
            return ((TYieldOf1)yields[0], (TYieldOf2)yields[1]);
        }

        public static async Task<(TYieldOf1, TYieldOf2)> Yields<TYieldOf1, TYieldOf2>(SeedBucketStartup seedBucketStartup, IOutputSink? outputSink = null)
        {
            var yields = await SeedAndGetYields(null, seedBucketStartup, outputSink, typeof(TYieldOf1), typeof(TYieldOf2));
            return ((TYieldOf1)yields[0], (TYieldOf2)yields[1]);
        }

        private static async Task<object[]> SeedAndGetYields(Type? seedBucketStartupType, SeedBucketStartup? seedBucketStartup, IOutputSink? outputSink, params Type[] yieldOfTypes)
        {
            // TODO: Assert type and startup object must not be specified at the same time.

            var output = outputSink ?? new InternalQueueOutputSink();
            var seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

            var seeder = new Seeder(seedBucketInfoBuilder, output);

            object[] result;

            if (seedBucketStartupType != null)
            {
                result = await seeder.GetYieldsFor(seedBucketStartupType, yieldOfTypes);
            }
            else if (seedBucketStartup != null)
            {
                result = await seeder.GetYieldsFor(seedBucketStartup, yieldOfTypes);
            }
            else
            {
                result = await seeder.GetYieldsFor(yieldOfTypes);
            }

            return result;
        }
    }
}
