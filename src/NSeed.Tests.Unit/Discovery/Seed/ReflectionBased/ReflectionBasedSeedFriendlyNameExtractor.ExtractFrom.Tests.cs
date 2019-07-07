using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Discovery.Seed;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public partial class ReflectionBasedSeedFriendlyNameExtractorﾠExtractFrom
    {
        private readonly ISeedFriendlyNameExtractor<Type> extractor = new ReflectionBasedSeedFriendlyNameExtractor();
        private readonly DistinctErrorCollectorAndProvider collector = new DistinctErrorCollectorAndProvider();

        [Fact]
        public void ExtractsﾠhumanizedﾠtypeﾠnameﾠwithoutﾠerrorsﾠwhenﾠtypeﾠdoesﾠnotﾠhaveﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithoutFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEmpty();
        }

        private class SeedWithoutFriendlyNameAttribute : BaseTestSeed { }

        private const string SomeFriendlyName = "Some friendly name";
        [Fact]
        public void ExtractsﾠdefinedﾠfriendlyﾠnameﾠwithoutﾠerrorsﾠwhenﾠtypeﾠhasﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(SomeFriendlyName);
            collector.GetErrors().Should().BeEmpty();
        }

        [FriendlyName(SomeFriendlyName)]
        private class SeedWithFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠnull()
        {
            Type type = typeof(SeedWithNullFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEquivalentTo(Errors.Seed.FriendlyName.MustNotBeNull);
        }

        [FriendlyName(null)]
        private class SeedWithNullFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠemptyﾠstring()
        {
            Type type = typeof(SeedWithEmptyStringFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEquivalentTo(Errors.Seed.FriendlyName.MustNotBeEmptyString);
        }

        [FriendlyName("")]
        private class SeedWithEmptyStringFriendlyNameAttribute : BaseTestSeed { }

        [Fact]
        public void ExtractsﾠhumanizedﾠtypeﾠnameﾠendﾠcollectﾠsingleﾠerrorﾠwhenﾠFriendlyNameﾠisﾠwhiteﾠspace()
        {
            Type type = typeof(SeedWithWhitespaceFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
            collector.GetErrors().Should().BeEquivalentTo(Errors.Seed.FriendlyName.MustNotBeWhitespace);
        }

        [FriendlyName("    ")]
        private class SeedWithWhitespaceFriendlyNameAttribute : BaseTestSeed { }
    }
}