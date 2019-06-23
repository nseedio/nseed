using System;
using System.Threading.Tasks;
using NSeed.Seeding;

namespace NSeed.Cli
{
    internal class SeedCommandExecutor : ICommandExecutor
    {
        public SeedVerbCommandOptions Options { get; }

        public SeedCommandExecutor(SeedVerbCommandOptions options) => Options = options;

        public async Task Execute<TSeedBucket>() where TSeedBucket : SeedBucket
        {
            var seeder = new Seeder();

            seeder.SeedingMessage += (_, args) => Console.WriteLine(args.Message + Environment.NewLine);
            seeder.Preparing      += (_, args) => Console.WriteLine("{0}. Preparing:      {1}", args.SeedingStep, args.Seed?.DisplayName);
            seeder.Seeding        += (_, args) => Console.WriteLine("{0}. Seeding:        {1}", args.SeedingStep, args.Seed?.DisplayName);
            seeder.SeedingEnded   += (_, args) => Console.WriteLine("{0}. Seeding ended:  {1}", args.SeedingStep, args.Seed?.DisplayName);
            seeder.Skipping       += (_, args) => Console.WriteLine("{0}. Skipping:       {1} (Output is already available)", args.SeedingStep, args.Seed?.DisplayName);
            seeder.SeedContractViolation += (_, args) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error:{0}{1}", Environment.NewLine, args.ErrorMessage);
                Console.ResetColor();
            };
            seeder.SeedingAborted += (_, args) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("{0}. Aborted:  {1}", args.SeedingStep, args.Seed?.DisplayName);
                Console.ResetColor();
            };

            try
            {
                await seeder.SeedFromBucket<TSeedBucket>(Options.Seeds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }
    }
}