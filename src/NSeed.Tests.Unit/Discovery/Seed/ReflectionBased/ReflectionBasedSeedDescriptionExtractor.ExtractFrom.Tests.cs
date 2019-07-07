using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Discovery.Seed;
using NSeed.MetaInfo;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public partial class ReflectionBasedSeedDescriptionExtractorﾠExtractFrom
    {
        private readonly ISeedDescriptionExtractor<Type> extractor = new ReflectionBasedSeedDescriptionExtractor();
        private readonly DistinctErrorCollectorAndProvider collector = new DistinctErrorCollectorAndProvider();

        [Fact]
        public void ExtractsﾠemptyﾠstringﾠwhenﾠtypeﾠdoesﾠnotﾠhaveﾠDescriptionﾠattribute()
        {
            Type type = typeof(SeedWithoutDescriptionAttribute);

            extractor.ExtractFrom(type, collector).Should().BeEmpty();
        }

        private const string SomeDescription = "Some description";
        [Fact]
        public void ExtractsﾠdefinedﾠdescriptionﾠwhenﾠtypeﾠhasﾠDescriptionﾠattribute()
        {
            Type type = typeof(SeedWithDescriptionAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(SomeDescription);
        }

        private class SeedWithoutDescriptionAttribute : BaseTestSeed { }

        [Description(SomeDescription)]
        private class SeedWithDescriptionAttribute : BaseTestSeed { }
    }
}