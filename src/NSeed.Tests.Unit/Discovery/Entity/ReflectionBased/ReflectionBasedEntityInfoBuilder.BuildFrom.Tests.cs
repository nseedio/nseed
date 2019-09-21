using FluentAssertions;
using NSeed.Discovery;
using NSeed.Discovery.Entity.ReflectionBased;
using NSeed.MetaInfo;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityInfoBuilderﾠBuildFrom
    {
        private readonly IMetaInfoBuilder<Type, EntityInfo> builder = new ReflectionBasedEntityInfoBuilder();

        [Fact]
        public void ReturnsﾠexpectedﾠfullyﾠpopulatedﾠEntityInfo()
        {
            Type type = typeof(string);

            var expected = new EntityInfo
            (
                type,
                type,
                type.FullName,
                Array.Empty<Error>()
            );

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ReturnsﾠexpectedﾠminimalﾠEntityInfo()
        {
            Type type = TestHelper.GetGenericTypeParameter();

            var expected = new EntityInfo
            (
                MetaInfo.MetaInfo.UnknownImplementation,
                null,
                string.Empty,
                Array.Empty<Error>()
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
