using FluentAssertions;
using NSeed.Cli.Abstractions;
using Xunit;

namespace NSeed.Cli.Tests.Unit.Entities
{
    public class ProjectTest
    {
        [Fact]
        public void ClassicTest()
        {
             var project = new Project(
                 @"C:\Repositories\Osobno\25.06.2019_NSeed\tests\smoke\TypicalSeedBucket.DotNetClassic\TypicalSeedBucket.DotNetClassic.csproj",
                 new Framework(Assets.FrameworkType.NETFramework, "2.0"));

             project.Name.Should().Be("TypicalSeedBucket");
        }

        [Fact]
        public void CoreTest()
        {
            var project = new Project(
                @"C:\Repositories\Osobno\25.06.2019_NSeed\tests\smoke\TypicalSeedBucket.DotNetCore\TypicalSeedBucket.DotNetCore.csproj",
                new Framework(Assets.FrameworkType.NETCoreApp, "3.0"));

            project.Name.Should().Be("TypicalSeedBucket");
        }
    }
}
