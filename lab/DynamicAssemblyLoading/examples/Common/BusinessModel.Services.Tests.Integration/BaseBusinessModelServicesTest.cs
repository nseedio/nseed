using Microsoft.Extensions.DependencyInjection;
using NSeed;
using NSeed.Seeding;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

#if DotNetCore
using Microsoft.EntityFrameworkCore;
using SmokeTests.Persistence.Repositories.EntityFrameworkCore;
#endif

#if DotNetClassic
using SmokeTests.Persistence.Repositories.EntityFramework;
#endif

namespace BusinessModel.Services.Tests.Integration
{
    public class BaseBusinessModelServicesTest
    {
        protected ObjectCreator ObjectCreator { get; private set; }

        [OneTimeSetUp]
        public void BaseOneTimeSetUp()
        {
            var seedingSetup = new IntegrationTestsSeedingSetup();
            var configuration = seedingSetup.BuildConfiguration(new string[0]);

            var serviceCollection = new ServiceCollection();
            seedingSetup.ConfigureServices(serviceCollection, configuration);

            ObjectCreator = new ObjectCreator(serviceCollection);
        }

        protected async Task Requires<TSeed>() where TSeed : ISeed
        {
            var seeder = new Seeder();

            seeder.SeedingMessage += (_, args) => Console.WriteLine(args.Message + Environment.NewLine);
            seeder.Preparing += (_, args) => Console.WriteLine("{0}. Preparing:      {1}", args.SeedingStep, args.Seed?.DisplayName);
            seeder.Seeding += (_, args) => Console.WriteLine("{0}. Seeding:        {1}", args.SeedingStep, args.Seed?.DisplayName);
            seeder.SeedingEnded += (_, args) => Console.WriteLine("{0}. Seeding ended:  {1}", args.SeedingStep, args.Seed?.DisplayName);
            seeder.Skipping += (_, args) => Console.WriteLine("{0}. Skipping:       {1} (Output is already available)", args.SeedingStep, args.Seed?.DisplayName);
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

            await seeder.Seed<TSeed, IntegrationTestsSeedingSetup>();
        }

        [TearDown]
        public void BaseTearDown()
        {
            var database = ObjectCreator.Create<SmokeTestsDbContext>().Database;

            DeleteTestUsers();

            void DeleteTestUsers()
            {
                database.ExecuteSqlCommand(@"DELETE FROM Users WHERE Username LIKE '%test'");
            }
        }
    }
}