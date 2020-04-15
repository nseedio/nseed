using Microsoft.Extensions.DependencyInjection;
using NSeed.Abstractions;
using NSeed.Discovery.SeedBucket;
using NSeed.Extensions;
using NSeed.Filtering;
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

        public Task<SeedingReport> SeedSeedBucket<TSeedBucket>()
            where TSeedBucket : SeedBucket
        {
            return SeedSeedBucket(typeof(TSeedBucket));
        }

        public Task<SeedingReport> SeedSeedBucket(Type seedBucketType, ISeedableFilter? filter = null) // TODO: Design the API properly, see where to have the filter in public methods etc.
        {
            System.Diagnostics.Debug.Assert(seedBucketType.IsSeedBucketType());

            // We know that the seed bucket info builder always returns
            // a seed bucket info and never null; threfore "!".
            var seedBucketInfo = seedBucketInfoBuilder.BuildFrom(seedBucketType)!;

            // TODO: Return proper seeding report if there is more then one seeding startup available.
            if (seedBucketInfo.Startups.Count() > 1) throw new Exception("TODO: Return proper seeding report if there is more then one seeding startup available.");

            var seedBucketStartupType = seedBucketInfo.Startups.FirstOrDefault().Type;

            return Seed(seedBucketInfo, CreateSeedBucketStartup(seedBucketStartupType), filter ?? AcceptAllSeedableFilter.Instance);
        }

        public Task<SeedingReport> SeedSeedBucket(Type seedBucketType, Type seedBucketStartupType, ISeedableFilter? filter = null) // TODO: Design API...
        {
            System.Diagnostics.Debug.Assert(seedBucketType.IsSeedBucketType());
            System.Diagnostics.Debug.Assert(seedBucketStartupType.IsSeedBucketStartupType());

            // We know that the seed bucket info builder always returns
            // a seed bucket info and never null; threfore "!".
            var seedBucketInfo = seedBucketInfoBuilder.BuildFrom(seedBucketType)!;

            return Seed(seedBucketInfo, CreateSeedBucketStartup(seedBucketStartupType), filter ?? AcceptAllSeedableFilter.Instance);
        }

        public Task<SeedingReport> SeedSeedBucket(Type seedBucketType, SeedBucketStartup seedBucketStartup, ISeedableFilter? filter = null) // TODO: Design API...
        {
            System.Diagnostics.Debug.Assert(seedBucketType.IsSeedBucketType());

            // We know that the seed bucket info builder always returns
            // a seed bucket info and never null; threfore "!".
            var seedBucketInfo = seedBucketInfoBuilder.BuildFrom(seedBucketType)!;

            return Seed(seedBucketInfo, seedBucketStartup, filter ?? AcceptAllSeedableFilter.Instance);
        }

        public Task<SeedingReport> SeedSeeds(params Type[] seedTypes)
        {
            // TODO: Checks. Not null, all seeds from the same seed bucket etc, must have seed bucket.
            // TODO: Workaround so far. Just pick up the first seed bucket.

            var seedBucketType = seedTypes.First().Assembly.GetTypes().First(type => typeof(SeedBucket).IsAssignableFrom(type));

            var filter = new FullNameEqualsSeedableFilter(seedTypes.Select(type => type.FullName).ToArray());

            return SeedSeedBucket(seedBucketType, filter);
        }

        public Task<SeedingReport> SeedSeeds(Type seedBucketStartupType, params Type[] seedTypes)
        {
            // TODO: Checks. Not null, all seeds from the same seed bucket etc, must have seed bucket.
            // TODO: Workaround so far. Just pick up the first seed bucket.

            var seedBucketType = seedTypes.First().Assembly.GetTypes().First(type => typeof(SeedBucket).IsAssignableFrom(type));

            var filter = new FullNameEqualsSeedableFilter(seedTypes.Select(type => type.FullName).ToArray());

            return SeedSeedBucket(seedBucketType, seedBucketStartupType, filter);
        }

        public Task<SeedingReport> SeedSeeds(SeedBucketStartup seedBucketStartup, params Type[] seedTypes)
        {
            // TODO: Checks. Not null, all seeds from the same seed bucket etc, must have seed bucket.
            // TODO: Workaround so far. Just pick up the first seed bucket.

            var seedBucketType = seedTypes.First().Assembly.GetTypes().First(type => typeof(SeedBucket).IsAssignableFrom(type));

            var filter = new FullNameEqualsSeedableFilter(seedTypes.Select(type => type.FullName).ToArray());

            return SeedSeedBucket(seedBucketType, seedBucketStartup, filter);
        }

        public Task<SeedingReport> SeedScenarios(params Type[] scenarioTypes)
        {
            // TODO: Checks. Not null, all scenarios from the same seed bucket etc, must have seed bucket.
            // TODO: Workaround so far. Just pick up the first seed bucket.

            var seedBucketType = scenarioTypes.First().Assembly.GetTypes().First(type => typeof(SeedBucket).IsAssignableFrom(type));

            var filter = new FullNameEqualsSeedableFilter(scenarioTypes.Select(type => type.FullName).ToArray());

            return SeedSeedBucket(seedBucketType);
        }

        public Task<SeedingReport> SeedScenarios(Type seedBucketStartupType, params Type[] scenarioTypes)
        {
            // TODO: Checks. Not null, all scenarios from the same seed bucket etc, must have seed bucket.
            // TODO: Workaround so far. Just pick up the first seed bucket.

            var seedBucketType = scenarioTypes.First().Assembly.GetTypes().First(type => typeof(SeedBucket).IsAssignableFrom(type));

            var filter = new FullNameEqualsSeedableFilter(scenarioTypes.Select(type => type.FullName).ToArray());

            return SeedSeedBucket(seedBucketType, seedBucketStartupType, filter);
        }

        public Task<SeedingReport> SeedScenarios(SeedBucketStartup seedBucketStartup, params Type[] scenarioTypes)
        {
            // TODO: Checks. Not null, all scenarios from the same seed bucket etc, must have seed bucket.
            // TODO: Workaround so far. Just pick up the first seed bucket.

            var seedBucketType = scenarioTypes.First().Assembly.GetTypes().First(type => typeof(SeedBucket).IsAssignableFrom(type));

            var filter = new FullNameEqualsSeedableFilter(scenarioTypes.Select(type => type.FullName).ToArray());

            return SeedSeedBucket(seedBucketType, seedBucketStartup, filter);
        }

        public Task<object[]> GetYieldsFor(params Type[] yieldOfTypes)
        {
            return GetYieldsFor((Type?)null, yieldOfTypes);
        }

        // TODO: What to return? SeedingReport? How to name the method so that it is clear that it seeds?
        public async Task<object[]> GetYieldsFor(Type? seedBucketStartupType, params Type[] yieldOfTypes)
        {
            // TODO: Checks. All yieldofs, no repeating etc.

            var result = new object[yieldOfTypes.Length];

            // TODO: Get the meta information, check it does not have any errors and out of it the seed types of these yields and seed only those seeds.
            // TODO: So far just grab the seed bucket and seed it all.
            var seedBucketType = yieldOfTypes.First().Assembly.GetTypes().First(type => typeof(SeedBucket).IsAssignableFrom(type));
            var seedTypes = yieldOfTypes.Select(yieldOfType => yieldOfType.BaseType.GetGenericArguments()[0]);
            var filter = new FullNameEqualsSeedableFilter(seedTypes.Select(type => type.FullName).ToArray());
            var seedingReport = seedBucketStartupType != null ? await SeedSeedBucket(seedBucketType, seedBucketStartupType, filter) : await SeedSeedBucket(seedBucketType, filter);
            if (seedingReport.Status != SeedingStatus.Succeeded) throw new Exception(); // TODO: Define how to return data and error status.

            // TODO: Brute force so far and a bunch of copy paste. In the production version refactor everything so that the needed objects are there.
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(outputSink);

            // TODO: Ugly workaround just to quickly get the seed bucket startup type. All this will need a super heavy refactoring one day it goes productive :-)
            if (seedBucketStartupType is null)
            {
                seedBucketStartupType = seedBucketType.Assembly.GetTypes().FirstOrDefault(type => type.IsSeedBucketStartupType());
            }

            if (seedBucketStartupType != null)
            {
                var startup = (SeedBucketStartup)ActivatorUtilities.GetServiceOrCreateInstance(serviceCollection.BuildServiceProvider(), seedBucketStartupType);

                startup.ConfigureServices(serviceCollection);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();
            for (var i = 0; i < yieldOfTypes.Length; i++)
            {
                var yieldOfType = yieldOfTypes[i];

                var yieldingSeed = (ISeed)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, yieldOfType.BaseType.GetGenericArguments()[0]);

                var yield = ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, yieldOfType);

                var seedPropertyOnYield = yield.GetType().GetProperty("Seed", BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                seedPropertyOnYield.SetValue(yield, yieldingSeed);

                result[i] = yield;
            }

            return result;
        }

        // TODO: A shameful copy-paste.
        public async Task<object[]> GetYieldsFor(SeedBucketStartup seedBucketStartup, params Type[] yieldOfTypes)
        {
            // TODO: Checks. All yieldofs, no repeating etc.

            var result = new object[yieldOfTypes.Length];

            // TODO: Get the meta information, check it does not have any errors and out of it the seed types of these yields and seed only those seeds.
            // TODO: So far just grab the seed bucket and seed it all.
            var seedBucketType = yieldOfTypes.First().Assembly.GetTypes().First(type => typeof(SeedBucket).IsAssignableFrom(type));
            var seedTypes = yieldOfTypes.Select(yieldOfType => yieldOfType.BaseType.GetGenericArguments()[0]);
            var filter = new FullNameEqualsSeedableFilter(seedTypes.Select(type => type.FullName).ToArray());
            var seedingReport = await SeedSeedBucket(seedBucketType, seedBucketStartup, filter);
            if (seedingReport.Status != SeedingStatus.Succeeded) throw new Exception(); // TODO: Define how to return data and error status.

            // TODO: Brute force so far and a bunch of copy paste. In the production version refactor everything so that the needed objects are there.
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(outputSink);

            seedBucketStartup.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            for (var i = 0; i < yieldOfTypes.Length; i++)
            {
                var yieldOfType = yieldOfTypes[i];

                var yieldingSeed = (ISeed)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, yieldOfType.BaseType.GetGenericArguments()[0]);

                var yield = ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, yieldOfType);

                var seedPropertyOnYield = yield.GetType().GetProperty("Seed", BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                seedPropertyOnYield.SetValue(yield, yieldingSeed);

                result[i] = yield;
            }

            return result;
        }

        private SeedBucketStartup? CreateSeedBucketStartup(Type? seedBucketStartupType)
        {
            // TODO: Checks or Debug.Asserts?
            if (seedBucketStartupType is null) return null;

            outputSink.WriteVerboseMessage($"Creating seed bucket startup of type {seedBucketStartupType}");

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(outputSink);

            return (SeedBucketStartup)ActivatorUtilities.GetServiceOrCreateInstance(serviceCollection.BuildServiceProvider(), seedBucketStartupType);
        }

        private async Task<SeedingReport> Seed(SeedBucketInfo seedBucketInfo, SeedBucketStartup? seedBucketStartup, ISeedableFilter filter)
        {
            if (seedBucketInfo.HasAnyErrors) return SeedingReport.CreateForSeedBucketHasErrors(seedBucketInfo);

            var seedingPlan = SeedingPlan.CreateFor(seedBucketInfo, filter);

            // TODO: Check if the seeding plan is empty and return appropriate report.
            //       In the report show the used filter. "... no seeds or scenarios that satisfied the given filter: ...". Add the FriendlyDescription property to the ISeedablesFilter.

            return await SeedSeedingPlan();

            async Task<SeedingReport> SeedSeedingPlan()
            {
                var singleSeedSeedingResults = new List<SingleSeedSeedingReport>();

                IServiceCollection serviceCollection = new ServiceCollection(); // TODO: We should have more of them. Think about lifecycle of the engine services, services in the different stages in the execution, etc.
                serviceCollection.AddSingleton(outputSink);
                try
                {
                    // TODO: Add creation of SeedingStartup and registration of services.
                    if (seedBucketStartup != null)
                    {
                        outputSink.WriteVerboseMessage($"Configuring service collection");

                        seedBucketStartup.ConfigureServices(serviceCollection);

                        // TODO: Check that the configuration does not override IOutputSink. Only the engine can add it. It must be singleton and the same one we have here.

                        outputSink.WriteVerboseMessage($"Initializing seeding");

                        using (var serviceScope = serviceCollection.BuildServiceProvider().CreateScope())
                        {
                            seedBucketStartup.InitializeSeeding(serviceScope.ServiceProvider);
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

                outputSink.WriteConfirmation($"Seeding completed");

                return SeedingReport.CreateForSucceeded(seedBucketInfo, seedingPlan, singleSeedSeedingResults);

                // Returns true if Seed() is called or false if the seed HasAlreadyYielded().
                async Task<bool> SeedSingleSeed(int seedingStep, IServiceProvider serviceProvider, SeedInfo seedInfo)
                {
                    System.Diagnostics.Debug.Assert(seedingStep > 0);

                    outputSink.WriteVerboseMessage($"Creating seed {seedInfo.FriendlyName}");

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
                        outputSink.WriteMessage($"Skipping {seedInfo.FriendlyName}");
                        return false;
                    }

                    outputSink.WriteMessage($"Seeding {seedInfo.FriendlyName}");

                    await seed.Seed();

                    outputSink.WriteMessage($"Seeding {seedInfo.FriendlyName} completed");

                    return true;
                }
            }
        }
    }
}
