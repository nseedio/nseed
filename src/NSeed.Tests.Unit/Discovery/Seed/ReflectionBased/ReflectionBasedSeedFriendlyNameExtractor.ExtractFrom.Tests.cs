using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery.Seed.ReflectionBased;
using NSeed.Discovery.Seed;
using NSeed.Guards;
using Humanizer;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    public partial class ReflectionBasedSeedFriendlyNameExtractorﾠExtractFromﾠTests
    {
        private ISeedFriendlyNameExtractor<Type> extractor = new ReflectionBasedSeedFriendlyNameExtractor();

        [Fact]
        public void Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull()
        {
            extractor.Invoking(x => x.ExtractFrom(null))
                .Should()
                .Throw<NSeedInternalErrorArgumentNullException>()
                .And.ParamName.Should().Be("seedImplementation");
        }

        [Fact]
        public void ShouldﾠextractﾠhumanizedﾠtypeﾠnameﾠwhenﾠtypeﾠdoesﾠnotﾠhaveﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithoutFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(type.Name.Humanize());
        }

        private const string SomeFriendlyName = "Some friendly name";
        [Fact]
        public void ShouldﾠextractﾠdefinedﾠfriendlyﾠnameﾠwhenﾠtypeﾠhasﾠFriendlyNameﾠattribute()
        {
            Type type = typeof(SeedWithFriendlyNameAttribute);

            extractor.ExtractFrom(type).Should().Be(SomeFriendlyName);
        }

        private class SeedWithoutFriendlyNameAttribute : BaseTestSeed { }

        [FriendlyName(SomeFriendlyName)]
        private class SeedWithFriendlyNameAttribute : BaseTestSeed { }
    }
}