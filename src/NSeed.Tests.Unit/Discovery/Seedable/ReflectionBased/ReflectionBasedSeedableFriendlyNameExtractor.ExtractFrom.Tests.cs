using System;
using Xunit;
using FluentAssertions;
using NSeed.Extensions;
using NSeed.MetaInfo;
using NSeed.Discovery.Seedable.ReflectionBased;
using NSeed.Discovery.Seedable;

namespace NSeed.Tests.Unit.Discovery.Seedable.ReflectionBased
{
    public partial class ReflectionBasedSeedableFriendlyNameExtractorﾠExtractFrom
    {
        private readonly ISeedableFriendlyNameExtractor<Type> extractor = new ReflectionBasedSeedableFriendlyNameExtractor();
        private readonly DistinctErrorCollectorAndProvider collector = new DistinctErrorCollectorAndProvider();

        [Fact]
        public void ExtractsﾠhumanizedﾠtypeﾠnameﾠwithoutﾠerrorsﾠwhenﾠseedﾠtypeﾠdoesﾠnotﾠhaveﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithoutFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEmpty();
        }
        private class SeedWithoutFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠtypeﾠnameﾠwithoutﾠerrorsﾠwhenﾠscenarioﾠtypeﾠdoesﾠnotﾠhaveﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(ScenarioWithoutFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEmpty();
        }
        private class ScenarioWithoutFriendlyNameAttribute : BaseTestScenario { }

        private const string SomeFriendlyName = "Some friendly name";
        [Fact]
        public void ExtractsﾠdefinedﾠfriendlyﾠnameﾠwithoutﾠerrorsﾠwhenﾠseedﾠtypeﾠhasﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(SomeFriendlyName);
            collector.GetErrors().Should().BeEmpty();
        }
        [FriendlyName(SomeFriendlyName)]
        private class SeedWithFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠdefinedﾠfriendlyﾠnameﾠwithoutﾠerrorsﾠwhenﾠscenarioﾠtypeﾠhasﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(ScenarioWithFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(SomeFriendlyName);
            collector.GetErrors().Should().BeEmpty();
        }
        [FriendlyName(SomeFriendlyName)]
        private class ScenarioWithFriendlyNameAttribute : BaseTestScenario { }

        [Fact]
        public void ExtractsﾠhumanizedﾠseedﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠnull()
        {
            Type type = typeof(SeedWithNullFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEquivalentTo(Errors.Seedable.FriendlyName.MustNotBeNull);
        }
        [FriendlyName(null)]
        private class SeedWithNullFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠscenarioﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠnull()
        {
            Type type = typeof(ScenarioWithNullFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEquivalentTo(Errors.Seedable.FriendlyName.MustNotBeNull);
        }
        [FriendlyName(null)]
        private class ScenarioWithNullFriendlyNameAttribute : BaseTestScenario { }

        [Fact]
        public void ExtractsﾠhumanizedﾠseedﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠemptyﾠstring()
        {
            Type type = typeof(SeedWithEmptyStringFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEquivalentTo(Errors.Seedable.FriendlyName.MustNotBeEmptyString);
        }
        [FriendlyName("")]
        private class SeedWithEmptyStringFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠscenarioﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠemptyﾠstring()
        {
            Type type = typeof(ScenarioWithEmptyStringFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEquivalentTo(Errors.Seedable.FriendlyName.MustNotBeEmptyString);
        }
        [FriendlyName("")]
        private class ScenarioWithEmptyStringFriendlyNameAttribute : BaseTestScenario { }

        [Fact]
        public void ExtractsﾠhumanizedﾠseedﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠwhiteﾠspace()
        {
            Type type = typeof(SeedWithWhitespaceFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEquivalentTo(Errors.Seedable.FriendlyName.MustNotBeWhitespace);
        }
        [FriendlyName("    ")]
        private class SeedWithWhitespaceFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠscenarioﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠwhiteﾠspace()
        {
            Type type = typeof(ScenarioWithWhitespaceFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEquivalentTo(Errors.Seedable.FriendlyName.MustNotBeWhitespace);
        }
        [FriendlyName("    ")]
        private class ScenarioWithWhitespaceFriendlyNameAttribute : BaseTestScenario { }
    }
}