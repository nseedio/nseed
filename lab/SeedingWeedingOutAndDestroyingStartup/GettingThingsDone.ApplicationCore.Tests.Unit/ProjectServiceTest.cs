using DotNetCoreSeeds;
using NSeed.Xunit;
using System;
using Xunit;
using Xunit.Abstractions;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    public class ProjectServiceTest
    {
        private readonly ITestOutputHelper output;

        public ProjectServiceTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Requires(typeof(DotNetCoreSeedsSeedBucket))]
        [Fact]
        public void Test1()
        {
            output.WriteLine($"Test {nameof(Test1)} is running");
        }
    }
}
