using Moq;
using System;
using Xunit;
using NSeed.Extensions;
using FluentAssertions;
using NSeed.Tests.Unit.Discovery.Seedable;

namespace NSeed.Tests.Unit.Extensions
{
    public class TypeExtensionsﾠIsSeedType
    {
        [Fact]
        public void ReturnsﾠtrueﾠwhenﾠtypeﾠdirectlyﾠimplementsﾠISeed()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            type.IsSeedType().Should().BeTrue();
        }

        [Fact]
        public void ReturnsﾠtrueﾠwhenﾠtypeﾠindirectlyﾠimplementsﾠISeed()
        {
            Type type = typeof(IndirectlyImplementsISeed);

            type.IsSeedType().Should().BeTrue();
        }
        private class IndirectlyImplementsISeed : BaseTestSeed { };

        [Fact]
        public void ReturnsﾠtrueﾠwhenﾠtypeﾠdirectlyﾠimplementsﾠISeedﾠofﾠaﾠsingleﾠentity()
        {
            Type type = new Mock<ISeed<string>>().Object.GetType();

            type.IsSeedType().Should().BeTrue();
        }

        [Fact]
        public void ReturnsﾠtrueﾠwhenﾠtypeﾠindirectlyﾠimplementsﾠISeedﾠofﾠaﾠsingleﾠentity()
        {
            Type type = typeof(IndirectlyImplementsISeedOfASingleEntity);

            type.IsSeedType().Should().BeTrue();
        }
        private class IndirectlyImplementsISeedOfASingleEntity : BaseTestSeed<string> { };

        [Fact]
        public void ReturnsﾠtrueﾠwhenﾠtypeﾠdirectlyﾠimplementsﾠISeedﾠofﾠtwoﾠentities()
        {
            Type type = new Mock<ISeed<string, int>>().Object.GetType();

            type.IsSeedType().Should().BeTrue();
        }

        [Fact]
        public void ReturnsﾠtrueﾠwhenﾠtypeﾠindirectlyﾠimplementsﾠISeedﾠofﾠtwoﾠentities()
        {
            Type type = typeof(IndirectlyImplementsISeedOfTwoEntities);

            type.IsSeedType().Should().BeTrue();
        }
        private class IndirectlyImplementsISeedOfTwoEntities : BaseTestSeed<string, object> { };

        [Fact]
        public void ReturnsﾠtrueﾠwhenﾠtypeﾠdirectlyﾠimplementsﾠISeedﾠofﾠthreeﾠentities()
        {
            Type type = new Mock<ISeed<string, int, double>>().Object.GetType();

            type.IsSeedType().Should().BeTrue();
        }

        [Fact]
        public void ReturnsﾠtrueﾠwhenﾠtypeﾠindirectlyﾠimplementsﾠISeedﾠofﾠthreeﾠentities()
        {
            Type type = typeof(IndirectlyImplementsISeedOfThreeEntities);

            type.IsSeedType().Should().BeTrue();
        }
        private class IndirectlyImplementsISeedOfThreeEntities : BaseTestSeed<string, object, int> { };

        [Fact]
        public void ReturnsﾠtrueﾠwhenﾠtypeﾠimplementsﾠseveralﾠISeedﾠinterfaces()
        {
            Type type = typeof(ImplementsSeveralISeedInterfaces);

            type.IsSeedType().Should().BeTrue();
        }
        private class ImplementsSeveralISeedInterfaces : BaseTestSeed<string, object, int>, ISeed, ISeed<string, object>, ISeed<int, long, short> { };

        [Fact]
        public void ReturnsﾠfalseﾠwhenﾠtypeﾠdoesﾠnotﾠimplementﾠISeedﾠinterface()
        {
            Type type = typeof(string);

            type.IsSeedType().Should().BeFalse();
        }
    }
}