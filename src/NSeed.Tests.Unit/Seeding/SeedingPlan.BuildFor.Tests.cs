using FluentAssertions;
using NSeed.Seeding;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace NSeed.Tests.Unit.Seeding
{
    public class SeedingPlanﾠBuildFor
    {
        private readonly Func<SeedAssemblyBuilder> createAssemblyBuilder;
        private readonly SeedAssemblyBuilder assemblyBuilder;

        public SeedingPlanﾠBuildFor(ITestOutputHelper output)
        {
            createAssemblyBuilder = () => new SeedAssemblyBuilder(output);
            assemblyBuilder = createAssemblyBuilder();
        }

        [Fact]
        public void Returnsﾠemptyﾠlistﾠwhenﾠseedﾠbucketﾠdoesﾠnotﾠcontainﾠanyﾠseeds()
        {
            var seedBucket = assemblyBuilder.AddSeedBucket().BuildAssembly().GetSeedBucket();

            var seedingPlan = SeedingPlan.CreateFor(seedBucket.GetMetaInfo());

            seedingPlan.SeedingSteps.Should().BeEmpty();
        }

        [Fact]
        public void Returnsﾠsingleﾠseedﾠwhenﾠseedﾠbucketﾠcontainsﾠonlyﾠoneﾠseed()
        {
            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("SingleSeed")
                .BuildAssembly()
                .GetSeedBucket();

            var seedingPlan = SeedingPlan.CreateFor(seedBucket.GetMetaInfo());

            seedingPlan.SeedingSteps
                .Should().ContainSingle(seedInfo => seedInfo.FullName.EndsWith("SingleSeed"));
        }

        [Fact]
        public void Returnsﾠexpectedﾠstepsﾠwhenﾠseedﾠbucketﾠcontainsﾠdisjunctﾠindependentﾠseeds()
        {
            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("FirstSeed")
                .AddSeed("SecondSeed")
                .AddSeed("ThirdSeed")
                .BuildAssembly()
                .GetSeedBucket();

            var seedingPlan = SeedingPlan.CreateFor(seedBucket.GetMetaInfo());

            seedingPlan.SeedingSteps
                .Select(seedInfo => seedInfo.FullName)
                .Should()
                .BeEquivalentTo("FirstSeed", "SecondSeed", "ThirdSeed");
        }

        [Fact]
        public void Returnsﾠexpectedﾠstepsﾠwhenﾠseedﾠbucketﾠcontainsﾠonlyﾠlinearﾠdependencies()
        {
            // The dependancy graph:
            // FirstSeed
            // |
            // SecondSeed
            // |
            // ThirdSeed

            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("FirstSeed")
                .AddSeed(seedTypeName: "SecondSeed", requires: "FirstSeed")
                .AddSeed(seedTypeName: "ThirdSeed", requires: "SecondSeed")
                .BuildAssembly()
                .GetSeedBucket();

            var seedingPlan = SeedingPlan.CreateFor(seedBucket.GetMetaInfo());

            seedingPlan.SeedingSteps
                .Select(seedInfo => seedInfo.FullName)
                .Should()
                .BeEquivalentTo(new[] { "FirstSeed", "SecondSeed", "ThirdSeed" }, options => options.WithStrictOrdering() );
        }

        [Fact]
        public void Returnsﾠexpectedﾠstepsﾠwhenﾠseedﾠbucketﾠcontainsﾠdiamondﾠdependency()
        {
            // The dependancy graph:
            // FirstSeed _
            // |          \
            // SecondSeed /
            // |         /
            // ThirdSeed

            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("FirstSeed")
                .AddSeed(seedTypeName: "SecondSeed", requires: "FirstSeed")
                .AddSeed(seedTypeName: "ThirdSeed", requires: new[] { "FirstSeed", "SecondSeed" })
                .BuildAssembly()
                .GetSeedBucket();

            var seedingPlan = SeedingPlan.CreateFor(seedBucket.GetMetaInfo());

            seedingPlan.SeedingSteps
                .Select(seedInfo => seedInfo.FullName)
                .Should()
                .BeEquivalentTo(new[] { "FirstSeed", "SecondSeed", "ThirdSeed" }, options => options.WithStrictOrdering());
        }

        [Fact]
        public void Returnsﾠexpectedﾠstepsﾠwhenﾠseedﾠbucketﾠcontainsﾠsingleﾠscenario()
        {
            // The dependancy graph:
            // FirstSeed _
            // |          \
            // SecondSeed /
            // |         /
            // ThirdSeed
            // |
            // Scenario

            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("FirstSeed")
                .AddSeed(seedTypeName: "SecondSeed", requires: "FirstSeed")
                .AddSeed(seedTypeName: "ThirdSeed", requires: new[] { "FirstSeed", "SecondSeed" })
                .AddScenario(scenarioTypeName: "Scenario", requires: "ThirdSeed")
                .BuildAssembly()
                .GetSeedBucket();

            var seedingPlan = SeedingPlan.CreateFor(seedBucket.GetMetaInfo());

            seedingPlan.SeedingSteps
                .Select(seedInfo => seedInfo.FullName)
                .Should()
                .BeEquivalentTo(new[] { "FirstSeed", "SecondSeed", "ThirdSeed" }, options => options.WithStrictOrdering());
        }

        [Fact]
        public void Returnsﾠexpectedﾠstepsﾠwhenﾠseedﾠbucketﾠcontainsﾠseveralﾠscenarios()
        {
            // The dependancy graph:
            // FirstSeed ____
            // |             \
            // SecondSeed    /
            // |            /
            // FirstScenario
            // |             \
            // ThirdSeed      |
            // |             /
            // SecondScenario

            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("FirstSeed")
                .AddSeed(seedTypeName: "SecondSeed", requires: "FirstSeed")
                .AddScenario(scenarioTypeName: "FirstScenario", requires: new[] { "FirstSeed", "SecondSeed" })
                .AddSeed(seedTypeName: "ThirdSeed", requires: "FirstScenario")
                .AddScenario(scenarioTypeName: "SecondScenario", requires: new[] { "ThirdSeed", "FirstScenario" })
                .BuildAssembly()
                .GetSeedBucket();

            var seedingPlan = SeedingPlan.CreateFor(seedBucket.GetMetaInfo());

            seedingPlan.SeedingSteps
                .Select(seedInfo => seedInfo.FullName)
                .Should()
                .BeEquivalentTo(new[] { "FirstSeed", "SecondSeed", "ThirdSeed" }, options => options.WithStrictOrdering());
        }

        [Fact]
        public void Returnsﾠexpectedﾠstepsﾠwhenﾠseedﾠbucketﾠcontainsﾠseveralﾠdisjunctﾠindependentﾠscenarios()
        {
            // The dependancy graph:
            // FirstSeed01 ____
            // |               \
            // SecondSeed01    /
            // |              /
            // FirstScenario01
            // |               \
            // ThirdSeed01      |
            // |               /
            // SecondScenario01
            //
            // FirstSeed02 ____
            // |               \
            // SecondSeed02    /
            // |              /
            // FirstScenario02
            // |               \
            // ThirdSeed02      |
            // |               /
            // SecondScenario02

            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("FirstSeed01")
                .AddSeed(seedTypeName: "SecondSeed01", requires: "FirstSeed01")
                .AddScenario(scenarioTypeName: "FirstScenario01", requires: new[] { "FirstSeed01", "SecondSeed01" })
                .AddSeed(seedTypeName: "ThirdSeed01", requires: "FirstScenario01")
                .AddScenario(scenarioTypeName: "SecondScenario01", requires: new[] { "ThirdSeed01", "FirstScenario01" })
                .AddSeed("FirstSeed02")
                .AddSeed(seedTypeName: "SecondSeed02", requires: "FirstSeed02")
                .AddScenario(scenarioTypeName: "FirstScenario02", requires: new[] { "FirstSeed02", "SecondSeed02" })
                .AddSeed(seedTypeName: "ThirdSeed02", requires: "FirstScenario02")
                .AddScenario(scenarioTypeName: "SecondScenario02", requires: new[] { "ThirdSeed02", "FirstScenario02" })
                .BuildAssembly()
                .GetSeedBucket();

            var seedingPlan = SeedingPlan.CreateFor(seedBucket.GetMetaInfo());

            seedingPlan.SeedingSteps
                .Select(seedInfo => seedInfo.FullName)
                .Should()
                .BeEquivalentTo("FirstSeed01", "SecondSeed01", "ThirdSeed01", "FirstSeed02", "SecondSeed02", "ThirdSeed02")
                .And
                .ContainInOrder("FirstSeed01", "SecondSeed01", "ThirdSeed01")
                .And
                .ContainInOrder("FirstSeed02", "SecondSeed02", "ThirdSeed02");
        }
    }
}
