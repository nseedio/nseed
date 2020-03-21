using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Seeding;
using System;

namespace NSeed.Xunit
{
    public abstract class RequiresScenariosFixture
    {
        protected RequiresScenariosFixture(Type? seedingStartupType, params Type[] scenarioTypes)
        {
            // TODO: Checks.

            var outputSink = new InternalQueueOutputSink();
            var seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

            var seeder = new Seeder(seedBucketInfoBuilder, outputSink);

            // TODO: Is not null, IsSeedBucketSeedingType etc.

            var seedingReport = seedingStartupType is null
                ? seeder.SeedScenarios(scenarioTypes).Result
                : seeder.SeedScenarios(seedingStartupType, scenarioTypes).Result;

            // TODO: See the best way to get the error message, inner exception and those kind of things. Throw some rich exception accordingly e.g. SeedingSingleSeedFaildException.
            if (seedingReport.Status != SeedingStatus.Succeeded) throw new Exception($"Seeding failed.{Environment.NewLine}{outputSink.GetOutputAsString()}");
        }
    }
}
