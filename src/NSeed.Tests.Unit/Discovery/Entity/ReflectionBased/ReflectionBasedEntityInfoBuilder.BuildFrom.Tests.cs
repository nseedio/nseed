using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery;
using NSeed.MetaInfo;
using NSeed.Discovery.Entity.ReflectionBased;
using static NSeed.Tests.Unit.Discovery.CommonReflectionBasedMetaInfoBuilderﾠBuildFromﾠTests;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityInfoBuilderﾠBuildFromﾠTests
    {
        private readonly IMetaInfoBuilder<Type, EntityInfo> builder = new ReflectionBasedEntityInfoBuilder();

        [Fact]
        public void Shouldﾠthrowﾠexceptionﾠwhenﾠimplementationﾠtypeﾠisﾠnull()
        {
            Shouldﾠthrowﾠexceptionﾠwhenﾠimplementationﾠtypeﾠisﾠnull<ReflectionBasedEntityInfoBuilder, EntityInfo>();
        }

        [Fact]
        public void ShouldﾠreturnﾠexpectedﾠfullyﾠpopulatedﾠEntityInfo()
        {
            Type type = typeof(string);

            var expected = new EntityInfo
            (
                type,
                type.FullName
            );

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldﾠreturnﾠexpectedﾠminimalﾠEntityInfo()
        {
            Type type = TestHelper.GetGenericTypeParameter();

            var expected = new EntityInfo
            (
                null,
                string.Empty
            );

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }
    }
}