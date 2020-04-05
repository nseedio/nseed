using DotNetCoreSeeds;
using GettingThingsDone.ApplicationCore.Services;
using GettingThingsDone.Contracts.Interface;
using NSeed.Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    [UseSeedingStartup(typeof(SampleStartupForUnitTests))]
    public class UsageOfRequiresAttributes
    {
        private readonly ITestOutputHelper output;
        private readonly ObjectCreator objectCreator = new ObjectCreator();

        public UsageOfRequiresAttributes(ITestOutputHelper output)
        {
            this.output = output;
        }

        [UseSeedingStartup(typeof(SampleStartupForUnitTests))]
        [RequiresSeedBucket(typeof(DotNetCoreSeedsSeedBucket))]
        [Fact]
        public async Task Test1()
        {
            output.WriteLine($"Test {nameof(Test1)} is running");

            var projectService = objectCreator.WithSharedInMemoryDatabase().Create<ProjectService>();

            var project = (await projectService.GetAll()).Value.First(project => project.Name.StartsWith("Mount Everest"));

            output.WriteLine(project.Name);
        }

        [RequiresSeedBucket(typeof(DotNetCoreSeedsSeedBucket), SeedingStartupType = typeof(FirstStartup))]
        [Fact]
        public async Task Test2()
        {
            output.WriteLine($"Test {nameof(Test2)} is running");

            var projectService = objectCreator.WithSqlServerDatabase().Create<ProjectService>();

            var project = (await projectService.GetAll()).Value.First(project => project.Name.StartsWith("Mount Everest"));

            output.WriteLine(project.Name);
        }

        [RequiresSeeds(typeof(MountEverestBaseCampTrackNextSteps), typeof(RentANewApartment))]
        [Fact]
        public void Test3()
        {
            output.WriteLine($"Test {nameof(Test3)} is running");
        }

        [RequiresScenarios(typeof(MountEverestScenario), SeedingStartupType = typeof(FirstStartup))]
        [Fact]
        public void Test4()
        {
            output.WriteLine($"Test {nameof(Test4)} is running");
        }
    }
}
