using Microsoft.Extensions.DependencyInjection;
using NSeed.Abstractions;
using NSeed.Discovery.SeedBucket;
using NSeed.Extensions;
using NSeed.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public Task<SeedingReport> Seed(Type seedBucketType)
        {
            System.Diagnostics.Debug.Assert(seedBucketType.IsSeedBucketType());

            // We know that the seed bucket info builder always returns
            // a seed bucket info and never null; threfore "!".
            var seedBucketInfo = seedBucketInfoBuilder.BuildFrom(seedBucketType)!;

            // TODO: Return proper seeding report if there is more then one seeding.
            if (seedBucketInfo.Startups.Count() > 1) throw new Exception("TODO: Return proper seeding report if there is more then one seeding.");

            var seedBucketStartupType = seedBucketInfo.Startups.FirstOrDefault().Type;

            return Seed(seedBucketInfo, seedBucketStartupType);
        }

        public Task<SeedingReport> Seed(Type seedBucketType, Type seedBucketStartupType)
        {
            System.Diagnostics.Debug.Assert(seedBucketType.IsSeedBucketType());
            System.Diagnostics.Debug.Assert(seedBucketStartupType.IsSeedBucketStartupType());

            // We know that the seed bucket info builder always returns
            // a seed bucket info and never null; threfore "!".
            var seedBucketInfo = seedBucketInfoBuilder.BuildFrom(seedBucketType)!;

            return Seed(seedBucketInfo, seedBucketStartupType);
        }

        private async Task<SeedingReport> Seed(SeedBucketInfo seedBucketInfo, Type? seedBucketStartupType)
        {
            if (seedBucketInfo.HasAnyErrors) return SeedingReport.CreateForSeedBucketHasErrors(seedBucketInfo);

            var seedingPlan = SeedingPlan.CreateFor(seedBucketInfo);

            return await SeedSeedingPlan();

            async Task<SeedingReport> SeedSeedingPlan()
            {
                var singleSeedSeedingResults = new List<SingleSeedSeedingReport>();

                IServiceCollection serviceCollection = new ServiceCollection(); // TODO: We should have more of them. Think about lifecycle of the engine services, services in the different stages in the execution, etc.
                serviceCollection.AddSingleton(outputSink);
                try
                {
                    // TODO: Add creation of SeedingStartup and registration of services.
                    if (seedBucketStartupType != null)
                    {
                        outputSink.WriteVerboseMessage($"Creating seed bucket startup of type {seedBucketStartupType} TODO");

                        var startup = (SeedBucketStartup)ActivatorUtilities.GetServiceOrCreateInstance(serviceCollection.BuildServiceProvider(), seedBucketStartupType);

                        outputSink.WriteVerboseMessage($"Configuring service collection TODO");

                        startup.ConfigureServices(serviceCollection);

                        // TODO: Check that the configuration does not override IOutputSink. Only the engine can add it. It must be singleton and the same one we have here.

                        outputSink.WriteVerboseMessage($"Initializing seeding TODO");

                        using (var serviceScope = serviceCollection.BuildServiceProvider().CreateScope())
                        {
                            startup.InitializeSeeding(serviceScope.ServiceProvider);
                        }
                    }

                    // TODO: Add finer granulation of errors and seeding result - CreatingSeedingStartupFailed.
                }
                catch (Exception exception)
                {
                    outputSink.WriteError(exception.ToString()); // TODO: Output sink for exceptions.

                    return SeedingReport.CreateForBuildingServiceProviderFailed(seedBucketInfo, seedingPlan);
                }

                var serviceProvider = serviceCollection.BuildServiceProvider();

                SeedInfo? currentSeedInfo;
                int seedingStepNumber;
                for (int i = 0; i < seedingPlan.SeedingSteps.Count; i++)
                {
                    currentSeedInfo = seedingPlan.SeedingSteps[i];
                    seedingStepNumber = i + 1;
                    try
                    {
                        bool hasSeeded;
                        using (var serviceScope = serviceProvider.CreateScope())
                        {
                            hasSeeded = await SeedSingleSeed(seedingStepNumber, serviceScope.ServiceProvider, currentSeedInfo);
                        }

                        singleSeedSeedingResults.Add(new SingleSeedSeedingReport(hasSeeded ? SingleSeedSeedingStatus.Seeded : SingleSeedSeedingStatus.Skipped, currentSeedInfo));
                    }
                    catch (Exception exception)
                    {
                        outputSink.WriteError(exception.ToString()); // TODO: Output sink that supports exceptions.

                        // TODO: Add finer granulation of errors and seeding result - CreatingSeedFailed.
                        // TODO: Add seed contract violation.
                        singleSeedSeedingResults.Add(new SingleSeedSeedingReport(SingleSeedSeedingStatus.Failed, currentSeedInfo));
                        return SeedingReport.CreateForSeedingSingleSeedFailed(seedBucketInfo, seedingPlan, singleSeedSeedingResults);
                    }
                }

                outputSink.WriteConfirmation($"Seeding completed TODO");

                return SeedingReport.CreateForSucceeded(seedBucketInfo, seedingPlan, singleSeedSeedingResults);

                // Returns true if Seed() is called or false if the seed HasAlreadyYielded().
                async Task<bool> SeedSingleSeed(int seedingStep, IServiceProvider serviceProvider, SeedInfo seedInfo)
                {
                    System.Diagnostics.Debug.Assert(seedingStep > 0);

                    outputSink.WriteVerboseMessage($"Creating seed {seedInfo.FriendlyName} TODO");

                    // TODO: Check if seedInfo.Type exists. Where to check that? Seeding should work only if Type exists. Where to add those checks?
                    var seed = (ISeed)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, seedInfo.Type);

                    // TODO: Create YieldOf objects and assign them to YieldOf properties.
                    // TODO: Think what to do if the Type or PropertyInfo is null.
                    foreach (var requiredYield in seedInfo.RequiredYields)
                    {
                        // TODO: Check if it is already created. If we have two properties of the same yield type. This must never be the case it should be an error in the seed definition.
                        // TOOD: Yield access property must be public or internal or protected. It also must have non private setter.

                        var yieldingSeed = (ISeed)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, requiredYield.YieldingSeed.Type);

                        var yield = ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, requiredYield.Type);

                        var seedPropertyOnYield = yield.GetType().GetProperty("Seed", BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                        // seedPropertyOnYield.SetValue(yield, yieldingSeed, BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.SetProperty, null, null, null);
                        seedPropertyOnYield.SetValue(yield, yieldingSeed);

                        requiredYield.YieldAccessProperty!.SetValue(seed, yield);
                    }

                    if (await seed.HasAlreadyYielded())
                    {
                        outputSink.WriteMessage($"Skipping {seedInfo.FriendlyName} TODO");
                        return false;
                    }

                    outputSink.WriteMessage($"Seeding {seedInfo.FriendlyName} TODO");

                    await seed.Seed();

                    outputSink.WriteMessage($"Seeding {seedInfo.FriendlyName} completed TODO");

                    return true;
                }
            }
        }
    }
}
