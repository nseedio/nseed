using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery;
using NSeed.MetaInfo;
using NSeed.Discovery.Entity.ReflectionBased;
using static NSeed.Tests.Unit.Discovery.CommonReflectionBasedMetaInfoBuilderﾠBuildFrom;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityInfoBuilderﾠBuildFrom
    {
        private readonly IMetaInfoBuilder<Type, EntityInfo> builder = new ReflectionBasedEntityInfoBuilder();

        [Fact]
        public void Throwsﾠexceptionﾠwhenﾠimplementationﾠtypeﾠisﾠnull()
        {
            Shouldﾠthrowﾠexceptionﾠwhenﾠimplementationﾠtypeﾠisﾠnull<ReflectionBasedEntityInfoBuilder, EntityInfo>();
        }

        [Fact]
        public void ReturnsﾠexpectedﾠfullyﾠpopulatedﾠEntityInfo()
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
        public void ReturnsﾠexpectedﾠminimalﾠEntityInfo()
        {
            Type type = TestHelper.GetGenericTypeParameter();

            var expected = new EntityInfo
            (
                null,
                string.Empty
            );

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ReturnsﾠexactlyﾠtheﾠsameﾠEntityInfoﾠforﾠtheﾠsameﾠentityﾠtype()
        {
            Type type01 = typeof(string);
            Type type02 = typeof(string);

            builder.BuildFrom(type01).Should().BeSameAs(builder.BuildFrom(type02));
        }
    }
}