using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Discovery.Seed;
using NSeed.Extensions;
using NSeed.MetaInfo;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public partial class ReflectionBasedSeedFriendlyNameExtractorﾠExtractFromﾠTests
    {
        private readonly ISeedFriendlyNameExtractor<Type> extractor = new ReflectionBasedSeedFriendlyNameExtractor();
        private readonly DistinctErrorCollectorAndProvider collector = new DistinctErrorCollectorAndProvider();

        [Fact]
        public void ShouldﾠextractﾠhumanizedﾠtypeﾠnameﾠwhenﾠtypeﾠdoesﾠnotﾠhaveﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithoutFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(type.Name.Humanize());
        }

        private const string SomeFriendlyName = "Some friendly name";
        [Fact]
        public void ShouldﾠextractﾠdefinedﾠfriendlyﾠnameﾠwhenﾠtypeﾠhasﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithFriendlyNameAttribute);

            extractor.ExtractFrom(type, collector).Should().Be(SomeFriendlyName);
        }

        private class SeedWithoutFriendlyNameAttribute : BaseTestSeed { }

        [FriendlyName(SomeFriendlyName)]
        private class SeedWithFriendlyNameAttribute : BaseTestSeed { }
    }
}