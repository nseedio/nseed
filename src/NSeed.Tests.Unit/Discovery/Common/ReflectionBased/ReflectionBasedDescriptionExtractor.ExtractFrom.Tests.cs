using System;
using Xunit;
using FluentAssertions;
using NSeed.MetaInfo;
using NSeed.Discovery;
using NSeed.Discovery.Common.ReflectionBased;
using NSeed.Tests.Unit.Discovery.Seedable;

namespace NSeed.Tests.Unit.Discovery.Common.ReflectionBased
{
    public partial class ReflectionBasedDescriptionExtractorﾠExtractFrom
    {
        private readonly IDescriptionExtractor<Type> extractor = new ReflectionBasedDescriptionExtractor();
        private readonly DistinctErrorCollectorAndProvider collector = new DistinctErrorCollectorAndProvider();

        [Fact]
        public void ExtractsﾠemptyﾠstringﾠwhenﾠseedﾠtypeﾠdoesﾠnotﾠhaveﾠDescriptionﾠattribute()
        {
            Type type = typeof(SeedWithoutDescriptionAttribute);

            extractor.ExtractFrom(type, collector).Should().BeEmpty();
        }
        private class SeedWithoutDescriptionAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠemptyﾠstringﾠwhenﾠscenarioﾠtypeﾠdoesﾠnotﾠhaveﾠDescriptionﾠattribute()
        {
            Type type = typeof(ScenarioWithoutDescriptionAttribute);

            extractor.ExtractFrom(type, collector).Should().BeEmpty();
        }
        private class ScenarioWithoutDescriptionAttribute : BaseTestScenario { }

        private const string SomeDescription = "Some description";
        [Fact]
        public void ExtractsﾠdefinedﾠdescriptionﾠwhenﾠseedﾠtypeﾠhasﾠDescriptionﾠattribute()
        {
            Type type = typeof(SeedWithDescriptionAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(SomeDescription);
        }
        [Description(SomeDescription)]
        private class SeedWithDescriptionAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠdefinedﾠdescriptionﾠwhenﾠscenarioﾠtypeﾠhasﾠDescriptionﾠattribute()
        {
            Type type = typeof(ScenarioWithDescriptionAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(SomeDescription);
        }
        [Description(SomeDescription)]
        private class ScenarioWithDescriptionAttribute : BaseTestScenario { }
    }
}