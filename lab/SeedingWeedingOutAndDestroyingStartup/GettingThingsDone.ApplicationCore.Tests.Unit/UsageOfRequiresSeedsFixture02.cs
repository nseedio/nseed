using DotNetCoreSeeds;
using NSeed.Xunit;
using System;
using Xunit;
using Xunit.Abstractions;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    public class UsageOfRequiresSeedsFixture02 : IClassFixture<UseSeeds<MountEverestBaseCampTrack, RentANewApartment>>
    {
        private readonly ITestOutputHelper output;

        public UsageOfRequiresSeedsFixture02(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test1()
        {
            output.WriteLine($"Test {nameof(Test1)} is running");
        }

        [Fact]
        public void Test2()
        {
            output.WriteLine($"Test {nameof(Test2)} is running");
        }

        [Fact]
        public void Test3()
        {
            output.WriteLine($"Test {nameof(Test3)} is running");
        }

        [Fact]
        public void Test4()
        {
            output.WriteLine($"Test {nameof(Test4)} is running");
        }
    }
}
