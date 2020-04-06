using NSeed.Abstractions;
using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Seeding;
using System;
using System.Threading.Tasks;

// TODO: Possible API:
// Seed.Seeds
// Seed.SeedBucket
// Seed.Scenarios
// Yield.Get  not consistent! should be Get.Yield
// -- or --
// Seeds.Seed
// SeedBucket.Seed
// Scenarios.Seed
// Yields.Get
// Should we use options builders instead of plain method parameters. Should we return some information instead of just Task?
// Should we have async and non async methods and suffix the async ones with async? It is confusing in tests and easy to forget to await.
// The options that we can have:
// - seeding startup type or object
// - seeding startup defined in place with lambdas
// - IOutputSink, WithVoidOutputSink, WithInMemoryBufferOutputSink, WithTestOutputHelperOutputSink

namespace NSeed.Sdk
{
    public static class Seed
    {
        public static Task Seeds<TSeed>()
            where TSeed : ISeed
        {
            return SeedSeeds(null, null, null, typeof(TSeed));
        }

        public static Task Seeds<TSeed1, TSeed2>()
            where TSeed1 : ISeed
            where TSeed2 : ISeed
        {
            return SeedSeeds(null, null, null, typeof(TSeed1), typeof(TSeed2));
        }

        public static Task Seeds<TSeed>(Type seedBucketStartupType)
            where TSeed : ISeed
        {
            return SeedSeeds(seedBucketStartupType, null, null, typeof(TSeed));
        }

        public static Task Seeds<TSeed1, TSeed2>(Type seedBucketStartupType)
            where TSeed1 : ISeed
            where TSeed2 : ISeed
        {
            return SeedSeeds(seedBucketStartupType, null, null, typeof(TSeed1), typeof(TSeed2));
        }

        public static Task Seeds<TSeed>(SeedBucketStartup seedBucketStartup)
            where TSeed : ISeed
        {
            return SeedSeeds(null, seedBucketStartup, null, typeof(TSeed));
        }

        public static Task Seeds<TSeed1, TSeed2>(SeedBucketStartup seedBucketStartup)
            where TSeed1 : ISeed
            where TSeed2 : ISeed
        {
            return SeedSeeds(null, seedBucketStartup, null, typeof(TSeed1), typeof(TSeed2));
        }

        public static Task Seeds<TSeed1, TSeed2>(SeedBucketStartup seedBucketStartup, IOutputSink outputSink)
            where TSeed1 : ISeed
            where TSeed2 : ISeed
        {
            return SeedSeeds(null, seedBucketStartup, outputSink, typeof(TSeed1), typeof(TSeed2));
        }

        private static async Task SeedSeeds(Type? seedBucketStartupType, SeedBucketStartup? seedBucketStartup, IOutputSink? outputSink, params Type[] seedTypes)
        {
            // TODO: Assert type and startup object must not be specified at the same time.

            var output = outputSink ?? new InternalQueueOutputSink();
            var seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

            var seeder = new Seeder(seedBucketInfoBuilder, output);

            SeedingReport seedingReport;

            if (seedBucketStartupType != null)
            {
                seedingReport = await seeder.SeedSeeds(seedBucketStartupType, seedTypes);
            }
            else if (seedBucketStartup != null)
            {
                seedingReport = await seeder.SeedSeeds(seedBucketStartup, seedTypes);
            }
            else
            {
                seedingReport = await seeder.SeedSeeds(seedTypes);
            }

            // TODO: See the best way to get the error message, inner exception and those kind of things. Throw some rich exception accordingly e.g. SeedingSingleSeedFaildException.
            var outputAsString = output is InternalQueueOutputSink internalOutputSink ? internalOutputSink.GetOutputAsString() : string.Empty;
            if (seedingReport.Status != SeedingStatus.Succeeded) throw new Exception($"Seeding failed.{Environment.NewLine}{outputAsString}");
        }
    }
}
