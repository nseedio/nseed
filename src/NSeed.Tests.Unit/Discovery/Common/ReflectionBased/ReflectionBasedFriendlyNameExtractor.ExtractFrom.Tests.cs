using FluentAssertions;
using NSeed.Discovery;
using NSeed.Discovery.Common.ErrorMessages;
using NSeed.Discovery.Common.ReflectionBased;
using NSeed.Extensions;
using NSeed.MetaInfo;
using NSeed.Tests.Unit.Discovery.Seedable;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Discovery.Common.ReflectionBased
{
    public partial class ReflectionBasedFriendlyNameExtractorﾠExtractFrom
    {
        private readonly IFriendlyNameExtractor<Type> extractor = new ReflectionBasedFriendlyNameExtractor();

        [Fact]
        public void ExtractsﾠhumanizedﾠtypeﾠnameﾠwithoutﾠerrorsﾠwhenﾠseedﾠtypeﾠdoesﾠnotﾠhaveﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithoutFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(type.Name.Humanize());
        }
        private class SeedWithoutFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠtypeﾠnameﾠwithoutﾠerrorsﾠwhenﾠscenarioﾠtypeﾠdoesﾠnotﾠhaveﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(ScenarioWithoutFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(type.Name.Humanize());
        }
        private class ScenarioWithoutFriendlyNameAttribute : BaseTestScenario { }

        private const string SomeFriendlyName = "Some friendly name";
        [Fact]
        public void ExtractsﾠdefinedﾠfriendlyﾠnameﾠwithoutﾠerrorsﾠwhenﾠseedﾠtypeﾠhasﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(SomeFriendlyName);
        }
        [FriendlyName(SomeFriendlyName)]
        private class SeedWithFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠdefinedﾠfriendlyﾠnameﾠwithoutﾠerrorsﾠwhenﾠscenarioﾠtypeﾠhasﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(ScenarioWithFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(SomeFriendlyName);
        }
        [FriendlyName(SomeFriendlyName)]
        private class ScenarioWithFriendlyNameAttribute : BaseTestScenario { }

        [Fact]
        public void ExtractsﾠhumanizedﾠseedﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠnull()
        {
            Type type = typeof(SeedWithNullFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(type.Name.Humanize());
        }
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        [FriendlyName(null)]
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        private class SeedWithNullFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠscenarioﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠnull()
        {
            Type type = typeof(ScenarioWithNullFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(type.Name.Humanize());
        }
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        [FriendlyName(null)]
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        private class ScenarioWithNullFriendlyNameAttribute : BaseTestScenario { }

        [Fact]
        public void ExtractsﾠhumanizedﾠseedﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠemptyﾠstring()
        {
            Type type = typeof(SeedWithEmptyStringFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(type.Name.Humanize());
        }
        [FriendlyName("")]
        private class SeedWithEmptyStringFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠscenarioﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠemptyﾠstring()
        {
            Type type = typeof(ScenarioWithEmptyStringFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(type.Name.Humanize());
        }
        [FriendlyName("")]
        private class ScenarioWithEmptyStringFriendlyNameAttribute : BaseTestScenario { }

        [Fact]
        public void ExtractsﾠhumanizedﾠseedﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠwhiteﾠspace()
        {
            Type type = typeof(SeedWithWhitespaceFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(type.Name.Humanize());
        }
        [FriendlyName("    ")]
        private class SeedWithWhitespaceFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠscenarioﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠwhiteﾠspace()
        {
            Type type = typeof(ScenarioWithWhitespaceFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(type.Name.Humanize());
        }
        [FriendlyName("    ")]
        private class ScenarioWithWhitespaceFriendlyNameAttribute : BaseTestScenario { }
    }
}
