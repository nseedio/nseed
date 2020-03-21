using DotNetCoreSeeds;
using NSeed.Xunit;
using System;
using Xunit;
using Xunit.Abstractions;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    [UseSeedingStartup(typeof(SampleStartupForUnitTests))]
    public class UsageOfRequiresAttributes
    {
        private readonly ITestOutputHelper output;

        public UsageOfRequiresAttributes(ITestOutputHelper output)
        {
            this.output = output;
        }

        [UseSeedingStartup(typeof(SampleStartupForUnitTests))]
        [RequiresSeedBucket(typeof(DotNetCoreSeedsSeedBucket))]
        [Fact]
        public void Test1()
        {
            output.WriteLine($"Test {nameof(Test1)} is running");
        }

        [RequiresSeedBucket(typeof(DotNetCoreSeedsSeedBucket), SeedingStartupType = typeof(FirstStartup))]
        [Fact]
        public void Test2()
        {
            output.WriteLine($"Test {nameof(Test2)} is running");
        }

        [RequiresSeeds(typeof(MountEverestBaseCampTrackNextSteps), typeof(RentANewApartment))]
        [Fact]
        public void Test3()
        {
            output.WriteLine($"Test {nameof(Test3)} is running");
        }

        [RequiresScenarios(typeof(MontEverestScenario), SeedingStartupType = typeof(FirstStartup))]
        [Fact]
        public void Test4()
        {
            output.WriteLine($"Test {nameof(Test4)} is running");
        }
    }
}
