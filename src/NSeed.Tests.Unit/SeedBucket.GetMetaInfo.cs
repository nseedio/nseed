using FluentAssertions;
using NSeed.Discovery.SeedBucket;
using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Extensions;
using NSeed.MetaInfo;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace NSeed.Tests.Unit
{
    public class SeedBucketﾠGetMetaInfo
    {
        private readonly Func<SeedAssemblyBuilder> createAssemblyBuilder;
        private readonly SeedAssemblyBuilder assemblyBuilder;
        private readonly IContainedSeedablesExtractor<Type> containedSeedablesExtractor = new ReflectionBasedContainedSeedablesExtractor();

        public SeedBucketﾠGetMetaInfo(ITestOutputHelper output)
        {
            createAssemblyBuilder = () => new SeedAssemblyBuilder(output);
            assemblyBuilder = createAssemblyBuilder();
        }

        [Fact]
        public void ReturnsﾠexpectedﾠminimalﾠSeedBucketInfo()
        {
            var seedBucket = assemblyBuilder.AddSeedBucket().BuildAssembly().GetSeedBucket();
            var type = seedBucket.GetType();

            var expected = new SeedBucketInfo
            (
                type,
                type,
                type.FullName,
                type.Name.Humanize(),
                string.Empty,
                Array.Empty<SeedableInfo>(),
                Array.Empty<Error>()
            );

            seedBucket.GetMetaInfo().Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ReturnsﾠexpectedﾠfullyﾠpopulatedﾠSeedBucketInfoﾠthatﾠdoesﾠnotﾠrequireﾠotherﾠseedﾠbuckets()
        {
            const string seedBucketTypeName = "TestSeedBucket";
            const string seedBucketFriendlyName = "Test seed bucket friendly name";
            const string seedBucketDescription = "Test seed bucket description";

            var seedBucket = assemblyBuilder
                .AddSeedBucket(seedBucketTypeName, seedBucketFriendlyName, seedBucketDescription)
                .AddSeed("FirstSeed")
                .AddSeed("SecondSeed", "Second seed friendly name", "Second seed description", true, "FirstSeed")
                .AddSeed("ThirdSeed", "Third seed friendly name", "Third seed description", true, "FirstSeed", "SecondSeed")
                .AddScenario("FirstScenario", requires: "FirstSeed")
                .AddScenario("SecondScenario", "Second scenario friendly name", "Second scenario description", "FirstSeed", "SecondSeed")
                .AddScenario("ThirdScenario", null, null, "FirstSeed", "SecondSeed", "FirstScenario", "SecondScenario")
                .AddSeed("FourthSeed", requires: "ThirdScenario")
                .BuildAssembly()
                .GetSeedBucket(seedBucketTypeName);
            var type = seedBucket.GetType();

            var expected = new SeedBucketInfo
            (
                type,
                type,
                type.FullName,
                seedBucketFriendlyName,
                seedBucketDescription,
                containedSeedablesExtractor.ExtractFrom(type, new DistinctErrorCollectorAndProvider()),
                Array.Empty<Error>()
            );

            var metaInfo = seedBucket.GetMetaInfo();

            metaInfo
                .Should()
                .BeEquivalentTo(expected, options => options.IgnoringCyclicReferences().WithoutStrictOrdering());

            metaInfo
                .ContainedSeedables
                .Select(seedable => seedable.SeedBucket)
                .Distinct()
                .Should()
                .HaveCount(1);

            metaInfo
                .ContainedSeedables
                .Select(seedable => seedable.SeedBucket)
                .First()
                .Should()
                .BeSameAs(metaInfo);
        }

        [Fact]
        public void ReturnsﾠexpectedﾠfullyﾠpopulatedﾠSeedBucketInfoﾠthatﾠrequiresﾠotherﾠseedﾠbuckets()
        {
            // The dependancy graph:
            // Base _      (has no SeedBucket)
            // |      \
            // Common  |   (has two SeedBuckets ;-))
            // |      \/
            // First  /    (has SeedBucket)
            // |     /
            // Second      (has SeedBucket)
            // |
            // SeedBucket  (has SeedBucket)

            var baseAssemblyReference = createAssemblyBuilder()
                .AddSeed("BaseSeed01")
                .AddSeed("BaseSeed02", requires: "BaseSeed01")
                .AddScenario("BaseScenario01", requires: "BaseSeed01")
                .AddScenario("BaseScenario02", requires: "BaseSeed02")
                .BuildPersistedAssembly()
                .GetMetadataReference();

            var commonAssemblyReference = createAssemblyBuilder()
                .AddReference(baseAssemblyReference)
                .AddSeedBucket("CommonSeedBucket01")
                .AddSeedBucket("CommonSeedBucket02")
                .AddSeed("CommonSeed01")
                .AddSeed("CommonSeed02", requires: "BaseSeed01")
                .AddScenario("CommonScenario01", requires: "CommonSeed01")
                .AddScenario("CommonScenario02", requires: "BaseSeed02")
                .BuildPersistedAssembly()
                .GetMetadataReference();

            var firstAssemblyReference = createAssemblyBuilder()
                .AddReferences(baseAssemblyReference, commonAssemblyReference)
                .AddSeedBucket("FirstSeedBucket")
                .AddSeed("FirstSeed01")
                .AddSeed("FirstSeed02", requires: "CommonSeed01")
                .AddScenario("FirstScenario01", requires: "FirstSeed01")
                .AddScenario("FirstScenario02", requires: "CommonSeed02")
                .BuildPersistedAssembly()
                .GetMetadataReference();

            var secondAssemblyReference = createAssemblyBuilder()
                .AddReferences(baseAssemblyReference, commonAssemblyReference, firstAssemblyReference)
                .AddSeedBucket("SecondSeedBucket")
                .AddSeed("SecondSeed01", requires: "FirstSeed01")
                .AddSeed("SecondSeed02", requires: "CommonSeed01")
                .AddScenario("SecondScenario01", requires: "FirstSeed01")
                .AddScenario("SecondScenario02", requires: "BaseSeed02")
                .BuildPersistedAssembly()
                .GetMetadataReference();

            const string seedBucketTypeName = "TestSeedBucket";
            const string seedBucketFriendlyName = "Test seed bucket friendly name";
            const string seedBucketDescription = "Test seed bucket description";

            var seedBucket = assemblyBuilder
                .AddReferences(baseAssemblyReference, commonAssemblyReference, firstAssemblyReference, secondAssemblyReference)
                .AddSeedBucket(seedBucketTypeName, seedBucketFriendlyName, seedBucketDescription)
                .AddSeed("FirstSeed", requires: "SecondSeed01")
                .AddSeed("SecondSeed", "Second seed friendly name", "Second seed description", true, "FirstSeed", "SecondSeed02")
                .AddSeed("ThirdSeed", "Third seed friendly name", "Third seed description", true, "FirstSeed", "SecondSeed", "SecondScenario01")
                .AddScenario("FirstScenario", requires: "FirstSeed")
                .AddScenario("SecondScenario", "Second scenario friendly name", "Second scenario description", "FirstSeed", "SecondSeed", "SecondScenario02")
                .AddScenario("ThirdScenario", null, null, "FirstSeed", "SecondSeed", "FirstScenario", "SecondScenario")
                .AddSeed("FourthSeed", requires: "ThirdScenario")
                .BuildAssembly()
                .GetSeedBucket(seedBucketTypeName);
            var type = seedBucket.GetType();

            var metaInfo = seedBucket.GetMetaInfo();

            // TODO: Put these two repeating checks into our own Should Assertions and get rid of copy-paste:
            //       containedSeedables.Should().HaveSeedBucket(SeedBucket seedBucket);
            //       containedSeedables.Should().HaveSeedBucketWithName(string seedBucketName);
            //       containedSeedables.Should().NotHaveSeedBucket();
            metaInfo
                .ContainedSeedables
                .Select(seedable => seedable.SeedBucket)
                .Distinct()
                .Should()
                .HaveCount(1);

            metaInfo
                .ContainedSeedables
                .Select(seedable => seedable.SeedBucket)
                .First()
                .Should()
                .BeSameAs(metaInfo);

            var nonContainedSeedables = SeedableInfo.GetRequiredSeedableInfosNotContainedIn(metaInfo.ContainedSeedables);

            var nonContainedSeedablesFromBase = nonContainedSeedables.Where(seedable => seedable.FullName.StartsWith("Base"));
            nonContainedSeedablesFromBase
                .Select(seedable => seedable.SeedBucket)
                .Distinct()
                .Should()
                .HaveCount(1);

            nonContainedSeedablesFromBase
                .Select(seedable => seedable.SeedBucket)
                .First()
                .Should()
                .BeNull();

            var nonContainedSeedablesFromCommon = nonContainedSeedables.Where(seedable => seedable.FullName.StartsWith("Common"));
            nonContainedSeedablesFromCommon
                .Select(seedable => seedable.SeedBucket)
                .Distinct()
                .Should()
                .HaveCount(1);

            nonContainedSeedablesFromCommon
                .Select(seedable => seedable.SeedBucket)
                .First()
                .Should()
                .BeNull();

            var nonContainedSeedablesFromFirst = nonContainedSeedables.Where(seedable => seedable.FullName.StartsWith("First"));
            nonContainedSeedablesFromFirst
                .Select(seedable => seedable.SeedBucket)
                .Distinct()
                .Should()
                .HaveCount(1);

            nonContainedSeedablesFromFirst
                .Select(seedable => seedable.SeedBucket!.FullName)
                .First()
                .Should()
                .Be("FirstSeedBucket");

            var nonContainedSeedablesFromSecond = nonContainedSeedables.Where(seedable => seedable.FullName.StartsWith("Second"));
            nonContainedSeedablesFromSecond
                .Select(seedable => seedable.SeedBucket)
                .Distinct()
                .Should()
                .HaveCount(1);

            nonContainedSeedablesFromSecond
                .Select(seedable => seedable.SeedBucket!.FullName)
                .First()
                .Should()
                .Be("SecondSeedBucket");
        }
    }
}
