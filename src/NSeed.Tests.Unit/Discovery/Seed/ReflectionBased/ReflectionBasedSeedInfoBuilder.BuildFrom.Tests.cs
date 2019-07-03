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
                    new EntityInfo(typeof(object), typeof(object).FullName),
                    new EntityInfo(typeof(string), typeof(string).FullName),
                    new EntityInfo(typeof(int), typeof(int).FullName)
                }
            );

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }

        private const string SomeFriendlyName = "Some friendly name";
        private const string SomeDescription = "Some description";
        [FriendlyName(SomeFriendlyName)]
        [Description(SomeDescription)]
        private class FullyPopulatedSeed : BaseTestSeed, ISeed<object, string, int> { }

        [Fact]
        public void ShouldﾠreturnﾠexpectedﾠminimalﾠSeedInfo()
        {
            Type type = typeof(MinimalSeed);

            var expected = new SeedInfo
            (
                type,
                type.FullName,
                type.Name.Humanize(),
                string.Empty,
                Array.Empty<EntityInfo>()
            );

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }

        private class MinimalSeed : BaseTestSeed { }
    }
}