using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Discovery.Seed;
using NSeed.Guards;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public class ReflectionBasedSeedTypeExtractorﾠExtractFromﾠTests
    {
        private ISeedTypeExtractor<Type> extractor = new ReflectionBasedSeedTypeExtractor();

        [Fact]
        public void Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull()
        {
            extractor.Invoking(x => x.ExtractFrom(null))
                .Should()
                .Throw<NSeedInternalErrorArgumentNullException>()
                .And.ParamName.Should().Be("seedImplementation");
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithoutﾠentities()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠoneﾠentity()
        {
            Type type = new Mock<ISeed<object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠtwoﾠentities()
        {
            Type type = new Mock<ISeed<object, object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠthreeﾠentities()
        {
            Type type = new Mock<ISeed<object, object, object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type);
        }
    }
}