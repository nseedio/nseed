using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Discovery.Seed;
using NSeed.Guards;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public class ReflectionBasedSeedFullNameExtractorﾠExtractFromﾠTests
    {
        private ISeedFullNameExtractor<Type> extractor = new ReflectionBasedSeedFullNameExtractor();

        [Fact]
        public void Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull()
        {
            extractor.Invoking(x => x.ExtractFrom(null))
                .Should()
                .Throw<NSeedInternalErrorArgumentNullException>()
                .And.ParamName.Should().Be("seedImplementation");
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithoutﾠentities()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠoneﾠentity()
        {
            Type type = new Mock<ISeed<object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠtwoﾠentities()
        {
            Type type = new Mock<ISeed<object, object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠthreeﾠentities()
        {
            Type type = new Mock<ISeed<object, object, object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }
    }
}