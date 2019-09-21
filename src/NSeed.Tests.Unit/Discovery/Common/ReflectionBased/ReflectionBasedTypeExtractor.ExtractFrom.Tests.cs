using FluentAssertions;
using Moq;
using NSeed.Discovery;
using NSeed.Discovery.Common.ReflectionBased;
using NSeed.MetaInfo;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Discovery.Common.ReflectionBased
{
    public class ReflectionBasedTypeExtractorﾠExtractFrom
    {
        private readonly ITypeExtractor<Type> extractor = new ReflectionBasedTypeExtractor();

        [Fact]
        public void Extractsﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithoutﾠentities()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Extractsﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠoneﾠentity()
        {
            Type type = new Mock<ISeed<object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Extractsﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠtwoﾠentities()
        {
            Type type = new Mock<ISeed<object, object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Extractsﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠthreeﾠentities()
        {
            Type type = new Mock<ISeed<object, object, object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Extractsﾠtypeﾠwhenﾠtypeﾠisﾠscenarioﾠtype()
        {
            Type type = new Mock<IScenario>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type);
        }
    }
}
