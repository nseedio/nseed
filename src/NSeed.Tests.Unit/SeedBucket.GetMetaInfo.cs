using Xunit;
using NSeed.Extensions;
using FluentAssertions;
using NSeed.MetaInfo;
using System;
using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Discovery.SeedBucket;
using Xunit.Abstractions;
using System.Linq;

namespace NSeed.Tests.Unit
{
    public class SeedBucketﾠGetMetaInfo
    {
        private readonly SeedAssemblyBuilder assemblyBuilder;
        private readonly IContainedSeedablesExtractor<Type> containedSeedablesExtractor = new ReflectionBasedContainedSeedablesExtractor();

        public SeedBucketﾠGetMetaInfo(ITestOutputHelper output)
        {
            assemblyBuilder = new SeedAssemblyBuilder(output);
        }

        [Fact]
        public void ReturnsﾠexpectedﾠminimalﾠSeedBucketInfo()
        {
            var seedBucket = assemblyBuilder.AddSeedBucket().BuildAndGetSeedBucket();
            var type = seedBucket.GetType();

            var expected = new SeedBucketInfo
            (
                type,
                type.FullName,
                type.Name.Humanize(),
                string.Empty,
                Array.Empty<SeedableInfo>()
            );

            seedBucket.GetMetaInfo().Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ReturnsﾠexpectedﾠfullyﾠpopulatedﾠSeedBucketInfo()
        {
            const string seedBucketTypeName = "TSB";
            const string seedBucketFriendlyName = "Test seed bucket";
            const string seedBucketDescription = "A test seed bucket full of seeds.";

            var seedBucket = assemblyBuilder
                .AddSeedBucket(seedBucketTypeName, seedBucketFriendlyName, seedBucketDescription)
                .AddSeed("FirstSeed")
                .AddSeed("SecondSeed", "Second seed", "Second seed seeding some seeds", true, "FirstSeed")
                .AddSeed("ThirdSeed", "Third seed", "Third seed seeding some seeds", true, "FirstSeed", "SecondSeed")
                .BuildAndGetSeedBucket(seedBucketTypeName);
            var type = seedBucket.GetType();

            var expected = new SeedBucketInfo
            (
                type,
                type.FullName,
                seedBucketFriendlyName,
                seedBucketDescription,
                containedSeedablesExtractor.ExtractFrom(type, new DistinctErrorCollectorAndProvider())
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
    }
}