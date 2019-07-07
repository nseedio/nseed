using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Discovery.Seed;
using NSeed.MetaInfo;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public class ReflectionBasedSeedFullNameExtractorﾠExtractFrom
    {
        private readonly ISeedFullNameExtractor<Type> extractor = new ReflectionBasedSeedFullNameExtractor();
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
    }
}