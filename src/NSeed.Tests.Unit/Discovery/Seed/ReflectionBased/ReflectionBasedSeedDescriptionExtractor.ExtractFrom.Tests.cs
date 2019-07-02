using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Discovery.Seed;
using static NSeed.Tests.Unit.Discovery.CommonReflectionBasedExtractorﾠExtractFromﾠTests;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public partial class ReflectionBasedSeedDescriptionExtractorﾠExtractFromﾠTests
    {
        private readonly ISeedDescriptionExtractor<Type> extractor = new ReflectionBasedSeedDescriptionExtractor();

        [Fact]
        public void Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull()
        {
            Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull<ReflectionBasedSeedDescriptionExtractor, string>("seed");
        }

        [Fact]
        public void ShouldﾠextractﾠemptyﾠstringﾠwhenﾠtypeﾠdoesﾠnotﾠhaveﾠDescriptionﾠattribute()
        {
            Type type = typeof(SeedWithoutDescriptionAttribute);

            extractor.ExtractFrom(type).Should().BeEmpty();
        }

        private const string SomeDescription = "Some description";
        [Fact]
        public void ShouldﾠextractﾠdefinedﾠdescriptionﾠwhenﾠtypeﾠhasﾠDescriptionﾠattribute()
        {
            Type type = typeof(SeedWithDescriptionAttribute);

            extractor.ExtractFrom(type).Should().Be(SomeDescription);
        }

        private class SeedWithoutDescriptionAttribute : BaseTestSeed { }

        [Description(SomeDescription)]
        private class SeedWithDescriptionAttribute : BaseTestSeed { }
    }
}