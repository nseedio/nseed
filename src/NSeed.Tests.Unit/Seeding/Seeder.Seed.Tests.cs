using FluentAssertions;
using Moq;
using NSeed.Abstractions;
using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Seeding;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace NSeed.Tests.Unit.Seeding
{
    public class SeederﾠSeed
    {
        private readonly Func<SeedAssemblyBuilder> createAssemblyBuilder;
        private readonly SeedAssemblyBuilder assemblyBuilder;
        private readonly Seeder seeder;

        public SeederﾠSeed(ITestOutputHelper output)
        {
            createAssemblyBuilder = () => new SeedAssemblyBuilder(output);
            assemblyBuilder = createAssemblyBuilder();
            seeder = new Seeder(new ReflectionBasedSeedBucketInfoBuilder(), new Mock<IOutputSink>().Object);
        }

        [Fact]
        public async Task ReturnsﾠSeedBucketHasErrorsﾠwhenﾠseedﾠbucketﾠhasﾠerrors()
        {
            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed(entities: new[] { "ISeed" })
                .BuildAssembly()
                .GetSeedBucket();

            var seedBucketInfo = seedBucket.GetMetaInfo();
            seedBucketInfo.HasAnyErrors.Should().BeTrue();

            var result = await seeder.Seed(seedBucket.GetType());

            result.Status.Should().BeEquivalentTo(SeedingStatus.SeedBucketHasErrors);
            result.SeedBucketInfo.Should().BeEquivalentTo(seedBucketInfo, option =>
                    option.IgnoringCyclicReferences());
            result.SeedingPlan.Should().BeNull();
            result.SingleSeedSeedingResults.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnsﾠSeedingSingleSeedFailedﾠwhenﾠfirstﾠseedﾠfails()
        {
            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed(fails: true)
                .BuildAssembly()
                .GetSeedBucket();

            var seedBucketInfo = seedBucket.GetMetaInfo();
            seedBucketInfo.HasAnyErrors.Should().BeFalse();

            var result = await seeder.Seed(seedBucket.GetType());

            result.Status.Should().BeEquivalentTo(SeedingStatus.SeedingSingleSeedFailed);
            result.SeedBucketInfo.Should().BeEquivalentTo(seedBucketInfo, option =>
                    option.IgnoringCyclicReferences());
            result.SeedingPlan.Should().BeEquivalentTo(SeedingPlan.CreateFor(seedBucketInfo), option =>
                    option.IgnoringCyclicReferences());
            result.SingleSeedSeedingResults
                .Should()
                .ContainSingle(seedingResult => seedingResult.Status == SingleSeedSeedingStatus.Failed);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ReturnsﾠSeedingSingleSeedFailedﾠwhenﾠsomeﾠseedsﾠsucceedﾠandﾠlastﾠoneﾠfails(bool hasAlreadyYielded)
        {
            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("FirstSeed", hasAlreadyYielded: hasAlreadyYielded)
                .AddSeed("SecondSeed", hasAlreadyYielded: hasAlreadyYielded, requires: "FirstSeed")
                .AddSeed("ThirdSeed", hasAlreadyYielded: hasAlreadyYielded, requires: "SecondSeed")
                .AddSeed("FailingSeed", fails: true, hasAlreadyYielded: false, requires: "ThirdSeed")
                .BuildAssembly()
                .GetSeedBucket();

            var seedBucketInfo = seedBucket.GetMetaInfo();
            seedBucketInfo.HasAnyErrors.Should().BeFalse();

            var result = await seeder.Seed(seedBucket.GetType());

            result.Status.Should().BeEquivalentTo(SeedingStatus.SeedingSingleSeedFailed);
            result.SeedBucketInfo.Should().BeEquivalentTo(seedBucketInfo, option =>
                    option.IgnoringCyclicReferences());
            result.SeedingPlan.Should().BeEquivalentTo(SeedingPlan.CreateFor(seedBucketInfo), option =>
                    option.IgnoringCyclicReferences());
            result.SingleSeedSeedingResults
                .Should()
                .HaveCount(4)
                .And
                .ContainSingle(seedingResult => seedingResult.Status == SingleSeedSeedingStatus.Failed)
                .Which
                .SeedInfo.FullName.Should().EndWith("FailingSeed");
            result.SingleSeedSeedingResults.Last().Status
                .Should().BeEquivalentTo(SingleSeedSeedingStatus.Failed);
            result.SingleSeedSeedingResults
                .Where(seedingResult => seedingResult.Status == (hasAlreadyYielded ? SingleSeedSeedingStatus.Skipped : SingleSeedSeedingStatus.Seeded))
                .Should()
                .HaveCount(3);
        }

        [Fact]
        public async Task ReturnsﾠSeedingSingleSeedFailedﾠwhenﾠsomeﾠseedsﾠsucceedﾠandﾠmiddleﾠoneﾠfails()
        {
            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("FirstSeed", hasAlreadyYielded: false)
                .AddSeed("SecondSeed", hasAlreadyYielded: true, requires: "FirstSeed")
                .AddSeed("ThirdSeed", hasAlreadyYielded: false, requires: "SecondSeed")
                .AddSeed("FailingSeed", fails: true, hasAlreadyYielded: false, requires: "ThirdSeed")
                .AddSeed("FourthSeed", hasAlreadyYielded: false, requires: "FailingSeed")
                .AddSeed("FifthSeed", hasAlreadyYielded: true, requires: "FourthSeed")
                .AddSeed("SixthSeed", hasAlreadyYielded: false, requires: "FifthSeed")
                .BuildAssembly()
                .GetSeedBucket();

            var seedBucketInfo = seedBucket.GetMetaInfo();
            seedBucketInfo.HasAnyErrors.Should().BeFalse();

            var result = await seeder.Seed(seedBucket.GetType());

            result.Status.Should().BeEquivalentTo(SeedingStatus.SeedingSingleSeedFailed);
            result.SeedBucketInfo.Should().BeEquivalentTo(seedBucketInfo, option =>
                    option.IgnoringCyclicReferences());
            result.SeedingPlan.Should().BeEquivalentTo(SeedingPlan.CreateFor(seedBucketInfo), option =>
                    option.IgnoringCyclicReferences());
            result.SingleSeedSeedingResults
                .Should()
                .HaveCount(4)
                .And
                .ContainSingle(seedingResult => seedingResult.Status == SingleSeedSeedingStatus.Failed)
                .Which
                .SeedInfo.FullName.Should().EndWith("FailingSeed");
            result.SingleSeedSeedingResults.Last().Status
                .Should().BeEquivalentTo(SingleSeedSeedingStatus.Failed);
            result.SingleSeedSeedingResults
                .Where(seedingResult => seedingResult.Status == SingleSeedSeedingStatus.Skipped)
                .Select(seedingResult => seedingResult.SeedInfo.FullName)
                .Should()
                .HaveCount(1)
                .And
                .Contain("SecondSeed");
            result.SingleSeedSeedingResults
                .Where(seedingResult => seedingResult.Status == SingleSeedSeedingStatus.Seeded)
                .Select(seedingResult => seedingResult.SeedInfo.FullName)
                .Should()
                .HaveCount(2)
                .And
                .Contain("FirstSeed", "ThirdSeed");
        }

        [Fact]
        public async Task ReturnsﾠSucceededﾠwhenﾠseedﾠbucketﾠhasﾠnoﾠseeds()
        {
            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .BuildAssembly()
                .GetSeedBucket();

            var seedBucketInfo = seedBucket.GetMetaInfo();
            seedBucketInfo.HasAnyErrors.Should().BeFalse();

            var result = await seeder.Seed(seedBucket.GetType());

            result.Status.Should().BeEquivalentTo(SeedingStatus.Succeeded);
            result.SeedBucketInfo.Should().BeEquivalentTo(seedBucketInfo, option =>
                    option.IgnoringCyclicReferences());
            result.SeedingPlan.Should().BeEquivalentTo(SeedingPlan.CreateFor(seedBucketInfo), option =>
                    option.IgnoringCyclicReferences());
            result.SingleSeedSeedingResults.Should().BeEmpty();
        }

        // TODO: Add test for BuildingServiceProviderFailed.

        [Fact]
        public async Task ReturnsﾠSucceededﾠwhenﾠsomeﾠseedsﾠyieldﾠandﾠsomeﾠskip()
        {
            var seedBucket = assemblyBuilder
                .AddSeedBucket()
                .AddSeed("FirstSeed", hasAlreadyYielded: false)
                .AddSeed("SecondSeed", hasAlreadyYielded: true, requires: "FirstSeed")
                .AddSeed("ThirdSeed", hasAlreadyYielded: false, requires: "SecondSeed")
                .AddSeed("FourthSeed", hasAlreadyYielded: true, requires: "ThirdSeed")
                .AddSeed("FifthSeed", hasAlreadyYielded: false, requires: "FourthSeed")
                .AddSeed("SixthSeed", hasAlreadyYielded: true, requires: "FifthSeed")
                .BuildAssembly()
                .GetSeedBucket();

            var seedBucketInfo = seedBucket.GetMetaInfo();
            seedBucketInfo.HasAnyErrors.Should().BeFalse();

            var result = await seeder.Seed(seedBucket.GetType());

            result.Status.Should().BeEquivalentTo(SeedingStatus.Succeeded);
            result.SeedBucketInfo.Should().BeEquivalentTo(seedBucketInfo, option =>
                    option.IgnoringCyclicReferences());
            result.SeedingPlan.Should().BeEquivalentTo(SeedingPlan.CreateFor(seedBucketInfo), option =>
                    option.IgnoringCyclicReferences());
            result.SingleSeedSeedingResults
                .Should()
                .HaveCount(6);
            result.SingleSeedSeedingResults
                .Where(seedingResult => seedingResult.Status == SingleSeedSeedingStatus.Skipped)
                .Select(seedingResult => seedingResult.SeedInfo.FullName)
                .Should()
                .HaveCount(3)
                .And
                .Contain("SecondSeed", "FourthSeed", "SixthSeed");
            result.SingleSeedSeedingResults
                .Where(seedingResult => seedingResult.Status == SingleSeedSeedingStatus.Seeded)
                .Select(seedingResult => seedingResult.SeedInfo.FullName)
                .Should()
                .HaveCount(3)
                .And
                .Contain("FirstSeed", "ThirdSeed", "FifthSeed");
        }
    }
}
