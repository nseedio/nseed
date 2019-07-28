using McMaster.Extensions.CommandLineUtils;
using NSeed.MetaInfo;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.Info
{
    [Command("info", Description = Resources.Info.CommandDescription)]
    internal class Subcommand
    {
        private readonly SeedBucket seedBucket;

        public Subcommand(SeedBucket seedBucket)
        {
            System.Diagnostics.Debug.Assert(seedBucket != null);

            this.seedBucket = seedBucket;
        }

        public Task OnExecute(CommandLineApplication app)
        {
            Console.WriteLine();

            var seedBucketInfo = seedBucket.GetMetaInfo();

            int numberOfSeeds = seedBucketInfo.ContainedSeedables.OfType<SeedInfo>().Count();
            int numberOfScenarios = seedBucketInfo.ContainedSeedables.OfType<ScenarioInfo>().Count();

            Console.WriteLine($"Summary");
            Console.WriteLine($"=======");
            Console.WriteLine($"Name:                {seedBucketInfo.FriendlyName}");
            Console.WriteLine($"Description:         {seedBucketInfo.Description}");
            Console.WriteLine($"Number of seeds:     {numberOfSeeds}");
            Console.WriteLine($"Number of scenarios: {numberOfScenarios}");

            if (numberOfSeeds > 0)
            {
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine($"Seeds");
                Console.WriteLine($"=====");
                int counter = 0;
                foreach (var seed in seedBucketInfo.ContainedSeedables.OfType<SeedInfo>())
                {
                    Console.WriteLine($"Name:             {seed.FriendlyName}");
                    Console.WriteLine($"Description:      {seed.Description}");
                    Console.WriteLine($"Creates entities: {string.Join(Environment.NewLine, seed.YieldedEntities.Select(entity => entity.FullName))}");
                    if (++counter != numberOfSeeds) Console.WriteLine("-----");
                }
            }

            if (numberOfScenarios > 0)
            {
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine($"Scenarios");
                Console.WriteLine($"=========");
                int counter = 0;
                foreach (var scenario in seedBucketInfo.ContainedSeedables.OfType<ScenarioInfo>())
                {
                    Console.WriteLine($"Name:             {scenario.FriendlyName}");
                    Console.WriteLine($"Description:      {scenario.Description}");
                    if (++counter != numberOfScenarios) Console.WriteLine("-----");
                }
            }

            Console.WriteLine();

            return Task.CompletedTask;
        }
    }
}
