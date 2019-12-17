using Microsoft.Extensions.DependencyInjection;
using NSeed.Abstractions;
using NSeed.Discovery.SeedBucket;
using NSeed.MetaInfo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSeed.Seeding
{
    internal class Seeder
    {
        private readonly ISeedBucketInfoBuilder<Type> seedBucketInfoBuilder;
        private readonly IOutputSink outputSink;

        public Seeder(ISeedBucketInfoBuilder<Type> seedBucketInfoBuilder, IOutputSink outputSink)
        {
            this.seedBucketInfoBuilder = seedBucketInfoBuilder;
            this.outputSink = outputSink;
        }

        public Task<SeedingReport> Seed<TSeedBucket>()
            where TSeedBucket : SeedBucket
        {
            return Seed(typeof(TSeedBucket));
        }

        public async Task<SeedingReport> Seed(Type seedBucketType)
        {
            System.Diagnostics.Debug.Assert(typeof(SeedBucket).IsAssignableFrom(seedBucketType));

            // We know that the seed bucket info builder always returns
            // a seed bucket info and never null; threfore "!".
            var seedBucketInfo = seedBucketInfoBuilder.BuildFrom(seedBucketType)!;
            if (seedBucketInfo.HasAnyErrors) return SeedingReport.CreateForSeedBucketHasErrors(seedBucketInfo);

            var seedingPlan = SeedingPlan.CreateFor(seedBucketInfo);

            return await SeedSeedingPlan();

            async Task<SeedingReport> SeedSeedingPlan()
            {
                SeedInfo? currentSeedInfo;
                int seedingStepNumber;
                var serviceCollection = new ServiceCollection();
                ServiceProvider? serviceProvider;
                var singleSeedSeedingResults = new List<SingleSeedSeedingReport>();

                try
                {
                    // TODO: Add creation of SeedingStartup and registration of services.

                    // TODO: Add finer granulation of errors and seeding result - CreatingSeedingStartupFailed.

                    serviceProvider = serviceCollection.BuildServiceProvider();
                }
                catch
                {
                    // TODO: Output sink.
                    return SeedingReport.CreateForBuildingServiceProviderFailed(seedBucketInfo, seedingPlan);
                }

                for (int i = 0; i < seedingPlan.SeedingSteps.Count; i++)
                {
                    currentSeedInfo = seedingPlan.SeedingSteps[i];
                    seedingStepNumber = i + 1;
                    try
                    {
                        var hasSeeded = await SeedSingleSeed(seedingStepNumber, serviceProvider, currentSeedInfo);
                        singleSeedSeedingResults.Add(new SingleSeedSeedingReport(hasSeeded ? SingleSeedSeedingStatus.Seeded : SingleSeedSeedingStatus.Skipped, currentSeedInfo));
                    }
                    catch
                    {
                        // TODO: Output sink.
                        // TODO: Add finer granulation of errors and seeding result - CreatingSeedFailed.
                        // TODO: Add seed contract violation.
                        singleSeedSeedingResults.Add(new SingleSeedSeedingReport(SingleSeedSeedingStatus.Failed, currentSeedInfo));
                        return SeedingReport.CreateForSeedingSingleSeedFailed(seedBucketInfo, seedingPlan, singleSeedSeedingResults);
                    }
                }

                return SeedingReport.CreateForSucceeded(seedBucketInfo, seedingPlan, singleSeedSeedingResults);

                // Returns true if Seed() is called or false if the seed HasAlreadyYielded().
                async Task<bool> SeedSingleSeed(int seedingStep, ServiceProvider serviceProvider, SeedInfo seedInfo)
                {
                    System.Diagnostics.Debug.Assert(seedingStep > 0);

                    // TODO: Output sink.

                    // TODO: Check if seedInfo.Type exists. Where to check that? Seeding should work only if Type exists. Where to add those checks?
                    var seed = (ISeed)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, seedInfo.Type);

                    // TODO: Create YieldOf objects and assign them to YieldOf properties.

                    if (await seed.HasAlreadyYielded())
                    {
                        // TODO: Output sink.
                        return false;
                    }

                    // TODO: Output sink.

                    await seed.Seed();

                    // TODO: Output sink.

                    return true;
                }
            }
        }
    }
}
