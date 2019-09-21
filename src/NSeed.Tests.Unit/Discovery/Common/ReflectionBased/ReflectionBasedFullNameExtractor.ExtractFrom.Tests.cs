using FluentAssertions;
using Moq;
using NSeed.Discovery;
using NSeed.Discovery.Common.ReflectionBased;
using NSeed.MetaInfo;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Discovery.Common.ReflectionBased
{
    public class ReflectionBasedFullNameExtractorﾠExtractFrom
    {
        private readonly IFullNameExtractor<Type> extractor = new ReflectionBasedFullNameExtractor();

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithoutﾠentities()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠoneﾠentity()
        {
            Type type = new Mock<ISeed<object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠtwoﾠentities()
        {
            Type type = new Mock<ISeed<object, object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠthreeﾠentities()
        {
            Type type = new Mock<ISeed<object, object, object>>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠscenarioﾠtype()
        {
            Type type = new Mock<IScenario>().Object.GetType();

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }
    }
}
