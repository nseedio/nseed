using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Sdk;
using NSeed.Seeding;
using System;

namespace NSeed.Xunit
{
    public abstract class RequiresSeedsFixture
    {
        protected RequiresSeedsFixture(Type? seedingStartupType, params Type[] seedTypes)
        {
            // TODO: Checks.

            var outputSink = new InternalQueueOutputSink();
            var seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

            var seeder = new Seeder(seedBucketInfoBuilder, outputSink);

            // TODO: Is not null, IsSeedBucketSeedingType etc.

            var seedingReport = seedingStartupType is null
                ? seeder.SeedSeeds(seedTypes).Result
                : seeder.SeedSeeds(seedingStartupType, seedTypes).Result;

            // TODO: See the best way to get the error message, inner exception and those kind of things. Throw some rich exception accordingly e.g. SeedingSingleSeedFaildException.
            if (seedingReport.Status != SeedingStatus.Succeeded) throw new Exception($"Seeding failed.{Environment.NewLine}{outputSink.GetOutputAsString()}");
        }
    }
}
