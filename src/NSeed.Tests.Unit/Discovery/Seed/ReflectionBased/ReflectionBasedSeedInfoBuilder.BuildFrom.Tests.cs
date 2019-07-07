using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery;
using NSeed.MetaInfo;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Extensions;
using static NSeed.Tests.Unit.Discovery.CommonReflectionBasedMetaInfoBuilderﾠBuildFromﾠTests;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public class ReflectionBasedSeedInfoBuilderﾠBuildFromﾠTests
    {
        private readonly IMetaInfoBuilder<Type, SeedInfo> builder = new ReflectionBasedSeedInfoBuilder();

        [Fact]
        public void Shouldﾠthrowﾠexceptionﾠwhenﾠimplementationﾠtypeﾠisﾠnull()
        {
            Shouldﾠthrowﾠexceptionﾠwhenﾠimplementationﾠtypeﾠisﾠnull<ReflectionBasedSeedInfoBuilder, SeedInfo>();
        }

        [Fact]
        public void ShouldﾠreturnﾠexpectedﾠfullyﾠpopulatedﾠSeedInfo()
        {
            Type type = typeof(FullyPopulatedSeed);

            var expected = new SeedInfo
            (
                type,
                type.FullName,
                SomeFriendlyName,
                SomeDescription,
                new []
                {
                    CreateSeedInfoForMinimalSeedType(typeof(MinimalSeed)),
                    CreateSeedInfoForMinimalSeedType(typeof(AdditionalMinimalSeed))
                },
                Array.Empty<SeedableInfo>(), // TODO-IG: Add Scenarios.
                new []
                {
                    new EntityInfo(typeof(object), typeof(object).FullName),
                    new EntityInfo(typeof(string), typeof(string).FullName),
                    new EntityInfo(typeof(int), typeof(int).FullName)
                }
            );

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }

        private const string SomeFriendlyName = "Some friendly name";
        private const string SomeDescription = "Some description";
        [Requires(typeof(AdditionalMinimalSeed))]
        [Requires(typeof(MinimalSeed))]
        // TODO-IG: Add Scenarios.
        [FriendlyName(SomeFriendlyName)]
        [Description(SomeDescription)]
        private class FullyPopulatedSeed : BaseTestSeed, ISeed<object, string, int> { }
        private class AdditionalMinimalSeed : BaseTestSeed { }

        [Fact]
        public void ShouldﾠreturnﾠexpectedﾠminimalﾠSeedInfo()
        {
            Type type = typeof(MinimalSeed);

            var expected = CreateSeedInfoForMinimalSeedType(type);

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }

        private class MinimalSeed : BaseTestSeed { }

        [Fact]
        public void ShouldﾠreturnﾠexactlyﾠtheﾠsameﾠSeedInfoﾠforﾠtheﾠsameﾠseedﾠtype()
        {
            Type type01 = typeof(MinimalSeed);
            Type type02 = typeof(MinimalSeed);

            builder.BuildFrom(type01).Should().BeSameAs(builder.BuildFrom(type02));
        }

        private static SeedInfo CreateSeedInfoForMinimalSeedType(Type minimalSeedType)
        {
            return new SeedInfo
            (
                minimalSeedType,
                minimalSeedType.FullName,
                minimalSeedType.Name.Humanize(),
                string.Empty,
                Array.Empty<SeedableInfo>(),
                Array.Empty<SeedableInfo>(),
                Array.Empty<EntityInfo>()
            );
        }
    }
}