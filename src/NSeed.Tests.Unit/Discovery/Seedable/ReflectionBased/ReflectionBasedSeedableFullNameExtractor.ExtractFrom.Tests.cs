using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSeed.MetaInfo;
using NSeed.Discovery.Seedable.ReflectionBased;
using NSeed.Discovery.Seedable;

namespace NSeed.Tests.Unit.Discovery.Seedable.ReflectionBased
{
    public class ReflectionBasedSeedableFullNameExtractorﾠExtractFrom
    {
        private readonly ISeedableFullNameExtractor<Type> extractor = new ReflectionBasedSeedableFullNameExtractor();
        private readonly DistinctErrorCollectorAndProvider collector = new DistinctErrorCollectorAndProvider();

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithoutﾠentities()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            extractor.ExtractFrom(type, collector).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠoneﾠentity()
        {
            Type type = new Mock<ISeed<object>>().Object.GetType();

            extractor.ExtractFrom(type, collector).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠtwoﾠentities()
        {
            Type type = new Mock<ISeed<object, object>>().Object.GetType();

            extractor.ExtractFrom(type, collector).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠthreeﾠentities()
        {
            Type type = new Mock<ISeed<object, object, object>>().Object.GetType();

            extractor.ExtractFrom(type, collector).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠtypeﾠfullﾠnameﾠwhenﾠtypeﾠisﾠscenarioﾠtype()
        {
            Type type = new Mock<IScenario>().Object.GetType();

            extractor.ExtractFrom(type, collector).Should().Be(type.FullName);
        }
    }
}