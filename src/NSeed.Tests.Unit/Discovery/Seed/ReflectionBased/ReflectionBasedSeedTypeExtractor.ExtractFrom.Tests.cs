using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Discovery.Seed;
using static NSeed.Tests.Unit.Discovery.CommonReflectionBasedExtractorﾠExtractFromﾠTests;
using NSeed.MetaInfo;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public class ReflectionBasedSeedTypeExtractorﾠExtractFromﾠTests
    {
        private readonly ISeedTypeExtractor<Type> extractor = new ReflectionBasedSeedTypeExtractor();
        private readonly DistinctErrorCollectorAndProvider collector = new DistinctErrorCollectorAndProvider();

        [Fact]
        public void Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull()
        {
            Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull<ReflectionBasedSeedTypeExtractor, Type>("seed");
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithoutﾠentities()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            extractor.ExtractFrom(type, collector).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠoneﾠentity()
        {
            Type type = new Mock<ISeed<object>>().Object.GetType();

            extractor.ExtractFrom(type, collector).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠtwoﾠentities()
        {
            Type type = new Mock<ISeed<object, object>>().Object.GetType();

            extractor.ExtractFrom(type, collector).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠseedﾠtypeﾠwithﾠthreeﾠentities()
        {
            Type type = new Mock<ISeed<object, object, object>>().Object.GetType();

            extractor.ExtractFrom(type, collector).Should().Be(type);
        }
    }
}