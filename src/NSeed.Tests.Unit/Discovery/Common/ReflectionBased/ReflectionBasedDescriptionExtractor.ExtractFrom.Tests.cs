using FluentAssertions;
using NSeed.Discovery;
using NSeed.Discovery.Common.ReflectionBased;
using NSeed.MetaInfo;
using NSeed.Tests.Unit.Discovery.Seedable;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Discovery.Common.ReflectionBased
{
    public partial class ReflectionBasedDescriptionExtractorﾠExtractFrom
    {
        private readonly IDescriptionExtractor<Type> extractor = new ReflectionBasedDescriptionExtractor();

        [Fact]
        public void ExtractsﾠemptyﾠstringﾠwhenﾠseedﾠtypeﾠdoesﾠnotﾠhaveﾠDescriptionﾠattribute()
        {
            Type type = typeof(SeedWithoutDescriptionAttribute);

            extractor.ExtractFrom(type).Should().BeEmpty();
        }
        private class SeedWithoutDescriptionAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠemptyﾠstringﾠwhenﾠscenarioﾠtypeﾠdoesﾠnotﾠhaveﾠDescriptionﾠattribute()
        {
            Type type = typeof(ScenarioWithoutDescriptionAttribute);

            extractor.ExtractFrom(type).Should().BeEmpty();
        }
        private class ScenarioWithoutDescriptionAttribute : BaseTestScenario { }

        private const string SomeDescription = "Some description";
        [Fact]
        public void ExtractsﾠdefinedﾠdescriptionﾠwhenﾠseedﾠtypeﾠhasﾠDescriptionﾠattribute()
        {
            Type type = typeof(SeedWithDescriptionAttribute);

            extractor.ExtractFrom(type).Should().Be(SomeDescription);
        }
        [Description(SomeDescription)]
        private class SeedWithDescriptionAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠdefinedﾠdescriptionﾠwhenﾠscenarioﾠtypeﾠhasﾠDescriptionﾠattribute()
        {
            Type type = typeof(ScenarioWithDescriptionAttribute);

            extractor.ExtractFrom(type).Should().Be(SomeDescription);
        }
        [Description(SomeDescription)]
        private class ScenarioWithDescriptionAttribute : BaseTestScenario { }
    }
}
