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
    public class UsageOfYieldDataAttribute
    {
        private readonly ITestOutputHelper output;
        private readonly ObjectCreator objectCreator = new ObjectCreator();

        public UsageOfYieldDataAttribute(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [YieldData]
        public async Task Test1(MountEverestBaseCampTrack.Yield baseCampTrack, RentANewApartment.Yield rentANewApartment)
        {
            output.WriteLine($"Test {nameof(Test1)} is running");

            var baseCampProject = await baseCampTrack.GetMountEverestBaseCampTrackProject();
            var newApartmentProject = await rentANewApartment.GetRentANewApartmentProject();

            output.WriteLine(baseCampProject.Name);
            output.WriteLine(newApartmentProject.Name);
        }
    }
}
