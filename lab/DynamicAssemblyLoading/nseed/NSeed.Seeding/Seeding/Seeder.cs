using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Contracts;
using static System.Diagnostics.Debug;

namespace NSeed.Seeding
{
    /// <summary>
    /// Seeds the seeds within a defined seeding.
    /// </summary>
    public class Seeder
    {
        private static readonly string[] EmptySeedFilter = new string[0];

        public event EventHandler<SeedingMessageEventArgs> SeedingMessage;

        public event EventHandler<SeedingEventArgs> Preparing;

        public event EventHandler<SeedingEventArgs> Seeding;

        public event EventHandler<SeedingEventArgs> SeedingEnded;

        public event EventHandler<SeedingEventArgs> Skipping;

        public event EventHandler<SeedContractViolationEventArgs> SeedContractViolation;

        public event EventHandler<SeedingEventArgs> SeedingAborted;


        public async Task SeedFromBucket<TSeedBucket>() where TSeedBucket : SeedBucket
        {
            await Seed(typeof(TSeedBucket).Assembly, EmptySeedFilter);
        }

        public async Task SeedFromBucket<TSeedBucket>(IEnumerable<string> seedFilters) where TSeedBucket : SeedBucket
        {
            await Seed(typeof(TSeedBucket).Assembly, seedFilters);
        }

        public async Task Seed<TSeed>() where TSeed : ISeed
        {
            await Seed(typeof(TSeed).Assembly, new[] { typeof(TSeed).FullName });
        }

        public async Task Seed<TSeed, TSeedingSetup>() where TSeed : ISeed where TSeedingSetup : ISeedingSetup
        {
            await Seed(typeof(TSeed).Assembly, new[] { typeof(TSeed).FullName }, typeof(TSeedingSetup));
        }

        public async Task Seed<TSeed>(ISeedingSetup seedingSetup) where TSeed : ISeed
        {
            await Seed(typeof(TSeed).Assembly, new[] { typeof(TSeed).FullName }, null, seedingSetup);
        }

        public SeedInfoTree GetSeedInfoTree(Assembly assembly)
        {
            return GetSeedInfoTree(assembly, EmptySeedFilter);
        }

        public SeedInfoTree GetSeedInfoTree(Assembly assembly, IEnumerable<string> seedFilters)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            return GetSeedInfoTreeImpl();

            SeedInfoTree GetSeedInfoTreeImpl()
            {
                var seedAssembly = SeedAssembly.Create(assembly, seedFilters);

                return SeedInfoTree.Create(seedAssembly.SeedTypes);
            }
        }

        public IEnumerable<SeedInfo> GetSeedsFor(Assembly assembly, IEnumerable<string> seedFilters)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            return GetSeedInfosImpl();

            IEnumerable<SeedInfo> GetSeedInfosImpl()
            {
                var seedAssembly = SeedAssembly.Create(assembly, seedFilters);

                return SeedInfoTree.Create(seedAssembly.SeedTypes)
                    .AllSeeds
                    .Where(seedInfo => seedAssembly.SeedTypes.Contains(seedInfo.SeedType));
            }
        }

        public SeedingPlan GetSeedingPlanFor(Assembly assembly, IEnumerable<string> seedFilters, ISeedingSetup seedingSetup = null)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            return GetSeedingPlanImpl();

            SeedingPlan GetSeedingPlanImpl()
            {
                return GetSeedingPlanFor(SeedAssembly.Create(assembly, seedFilters), seedingSetup);
            }
        }

        private static SeedingPlan GetSeedingPlanFor(SeedAssembly seedAssembly, ISeedingSetup seedingSetup)
        {
            Assert(seedAssembly != null);

            var seedInfoTree = SeedInfoTree.Create(seedAssembly.SeedTypes);

            var seedingSteps = new List<SeedInfo>();
            foreach (var seedInfo in seedInfoTree.AllSeeds)
                RecursivelyBuildSeedingSteps(seedInfo);

            return new SeedingPlan(seedingSetup, seedAssembly.SeedingSetupType, seedingSteps);

            void RecursivelyBuildSeedingSteps(SeedInfo seedInfo)
            {
                if (seedingSteps.Contains(seedInfo)) return;

                foreach (var dependency in seedInfo.DependsOn)
                    RecursivelyBuildSeedingSteps(dependency);

                seedingSteps.Add(seedInfo);
            }
        }

        public async Task Seed(Assembly assembly, IEnumerable<string> seedFilters, Type seedingSetupType = null, ISeedingSetup seedingSetup = null)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            await SeedImpl();

            async Task SeedImpl()
            {
                var filters = seedFilters.ToArray();

                RaiseMessageEvent(string.Format("Seed assembly: {1}{0}" +
                                                "Seed filter:{0}{2}",
                                                Environment.NewLine,
                                                assembly.Location,
                                                string.Join(Environment.NewLine, filters.Select(filter => "\t" + filter))));

                var seedAssembly = SeedAssembly.Create(assembly, filters, seedingSetupType);

                var seedingPlan = GetSeedingPlanFor(seedAssembly, seedingSetup);

                RaiseMessageEvent(string.Format("Seeding plan:{0}{1}",
                                                Environment.NewLine,
                                                string.Join(Environment.NewLine, seedingPlan.Seeds.Select((seedInfo, index) => (index + 1) + ". " + seedInfo.DisplayName))));

                RaiseMessageEvent("Seeding started.");

                try
                {
                    await Seed(seedingPlan);
                }
                catch (Exception e)
                {
                    RaiseMessageEvent(e.ToString());
                }
                

                RaiseMessageEvent("Seeding finished.");
            }
        }

        private void RaiseMessageEvent(string message)
        {
            SeedingMessage?.Invoke(this, new SeedingMessageEventArgs(message));
        }

        private async Task Seed(SeedingPlan seedingPlan)
        {
            SeedInfo currentSeedInfo = null;
            int seedingStep = 0;
            try
            {
                var serviceCollection = new ServiceCollection();

                var seedingSetup = seedingPlan.SeedingSetup;

                if (seedingSetup == null && seedingPlan.SeedingSetupType != null)
                {
                    seedingSetup = (ISeedingSetup) Activator.CreateInstance(seedingPlan.SeedingSetupType);
                }

                if (seedingSetup != null)
                {
                    RaiseMessageEvent($"Configuring services by using the seeding setup '{seedingPlan.SeedingSetupType}'.");
                    seedingSetup.ConfigureServices(serviceCollection, seedingSetup.BuildConfiguration(new string[0]));
                    RaiseMessageEvent("Services configured.");
                }
                else
                {
                    RaiseMessageEvent($"No seeding setup found.");
                }

                var serviceProvider = serviceCollection.BuildServiceProvider();

                for (int i = 0; i < seedingPlan.Seeds.Count; i++)
                {
                    currentSeedInfo = seedingPlan.Seeds[i];
                    seedingStep = i + 1;
                    await SeedSingleSeed(seedingStep, serviceProvider, currentSeedInfo);
                }
            }
            catch (SeedContractViolationException exception)
            {
                SeedContractViolation?.Invoke(this, new SeedContractViolationEventArgs(seedingStep, currentSeedInfo, exception.Message));
                SeedingAborted?.Invoke(this, new SeedingEventArgs(seedingStep, currentSeedInfo));
            }
            catch (Exception exception)
            {
                RaiseMessageEvent($"Exception: {exception}");
                SeedingAborted?.Invoke(this, new SeedingEventArgs(seedingStep, currentSeedInfo));
            }
        }

        private async Task SeedSingleSeed(int seedingStep, ServiceProvider serviceProvider, SeedInfo seedInfo)
        {
            Assert(seedingStep > 0);
            Assert(serviceProvider != null);
            Assert(seedInfo != null);

            Preparing?.Invoke(this, new SeedingEventArgs(seedingStep, seedInfo));

            var seed = (ISeed)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, seedInfo.SeedType);
            foreach (var seedOutputProperty in seedInfo.SeedOutputProperties)
            {
                // We always want to create a new instance of a seed output class.
                // Why? Because in a general case seed output classes will have dependency constructors
                // that can potentially have transient dependencies.
                var propertyValue = ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, seedOutputProperty.PropertyType);
                seedOutputProperty.SetValue(seed, propertyValue);
            }

            if (await seed.OutputAlreadyExists())
            {
                Skipping?.Invoke(this, new SeedingEventArgs(seedingStep, seedInfo));
                return;
            }

            Seeding?.Invoke(this, new SeedingEventArgs(seedingStep, seedInfo));

            await seed.Seed();

            SeedingEnded?.Invoke(this, new SeedingEventArgs(seedingStep, seedInfo));
        }
    }
}