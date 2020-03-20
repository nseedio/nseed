using DotNetCoreSeeds;
using NSeed.Xunit;
using System;
using Xunit;
using Xunit.Abstractions;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    [UseSeedingStartup(typeof(SampleStartupForUnitTests))]
    public class ProjectServiceTest
    {
        private readonly ITestOutputHelper output;

        public ProjectServiceTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [RequiresSeedBucket(typeof(DotNetCoreSeedsSeedBucket))]
        [Fact]
        public void Test1()
        {
            output.WriteLine($"Test {nameof(Test1)} is running");
        }

        [RequiresSeedBucket(typeof(DotNetCoreSeedsSeedBucket))]
        [Fact]
        public void Test2()
        {
            output.WriteLine($"Test {nameof(Test2)} is running");
        }
    }
}
