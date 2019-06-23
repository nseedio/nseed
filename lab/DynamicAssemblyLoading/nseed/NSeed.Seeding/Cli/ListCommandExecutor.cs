using System.Linq;
using System.Threading.Tasks;
using NSeed.Seeding;
using static System.Console;

namespace NSeed.Cli
{
    internal class ListCommandExecutor : ICommandExecutor
    {
        public ListVerbCommandOptions Options { get; }

        public ListCommandExecutor(ListVerbCommandOptions options) => Options = options;

        public Task Execute<TSeedBucket>() where TSeedBucket : SeedBucket
        {
            var seeder = new Seeder();
            
            var seeds = seeder.GetSeedsFor(typeof(TSeedBucket).Assembly, Options.Seeds)
                .Select(seedInfo => seedInfo.DisplayName)
                .OrderBy(name => name)
                .ToArray();

            WriteLine();

            if (seeds.Length <= 0)
            {
                WriteLine("There are no seeds in the given seed assembly that satisfy the given seed filter.");
            }
            else
            {
                WriteLine("Seeds that satisfy the seed filter:");
                foreach (var seed in seeds)
                    WriteLine(seed);

                WriteLine();

                WriteLine("Seeding plan:");
                var seedingPlan = seeder.GetSeedingPlanFor(typeof(TSeedBucket).Assembly, Options.Seeds);
                for (int i = 0; i < seedingPlan.Seeds.Count; i++)
                    WriteLine("{0}. {1}", i + 1, seedingPlan.Seeds[i].DisplayName);
            }

            WriteLine();

            return Task.CompletedTask;
        }
    }
}