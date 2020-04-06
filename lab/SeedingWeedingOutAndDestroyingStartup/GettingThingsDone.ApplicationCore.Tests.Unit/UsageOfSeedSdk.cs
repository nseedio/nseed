using DotNetCoreSeeds;
using GettingThingsDone.ApplicationCore.Services;
using GettingThingsDone.Contracts.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using NSeed.Sdk;
using NSeed.Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    public class UsageOfSeedSdk
    {
        private readonly ITestOutputHelper output;
        private readonly ObjectCreator objectCreator = new ObjectCreator();

        public UsageOfSeedSdk(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task Test1()
        {
            output.WriteLine($"Test {nameof(Test1)} is running");

            // Seed seed using the default startup found in the bucket.
            await Seed.Seeds<MountEverestBaseCampTrackNextSteps, RentANewApartment>();
        }

        [Fact]
        public async Task Test2()
        {
            output.WriteLine($"Test {nameof(Test2)} is running");

            // Seed seeds using the specified startup type.
            await Seed.Seeds<MountEverestBaseCampTrackNextSteps, RentANewApartment>(typeof(FirstStartup));
        }

        [Fact]
        public async Task Test3()
        {
            output.WriteLine($"Test {nameof(Test3)} is running");

            // Seed seeds using the specified startup type.
            await Seed.Seeds<MountEverestBaseCampTrackNextSteps, RentANewApartment>(typeof(FirstStartup));
        }

        [Fact]
        public async Task Test4()
        {
            output.WriteLine($"Test {nameof(Test4)} is running");

            // Seed seeds using the specified startup object.
            // This way we can provide data from the test to the startup.
            // Typical case, we want the inmemory database to be unique per test.
            // We will create the database root in the test and provide it both to
            // the startup and to the object creator.

            var databaseName = Guid.NewGuid().ToString();
            var databaseRoot = new InMemoryDatabaseRoot();

            var outputSink = new InternalQueueOutputSink();
            var startup = new SampleStartupForUnitTests(outputSink)
            {
                DatabaseName = databaseName,
                DatabaseRoot = databaseRoot
            };

            await Seed.Seeds<MountEverestBaseCampTrackNextSteps, RentANewApartment>(startup, outputSink);

            output.WriteLine(outputSink.GetOutputAsString());

            var projectService = objectCreator.WithLocalInMemoryDatabase(databaseName, databaseRoot).Create<IProjectService>();
            var projects = (await projectService.GetAll()).Value;
            Assert.Equal(2, projects.Count);

            projectService = objectCreator.WithSharedInMemoryDatabase().Create<IProjectService>();
            projects = (await projectService.GetAll()).Value;
            Assert.Empty(projects);

            projectService = objectCreator.WithLocalInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot()).Create<IProjectService>();
            projects = (await projectService.GetAll()).Value;
            Assert.Empty(projects);
        }
    }
}
