using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSeed.Tests.Unit.Discovery.Seed;
using NSeed.Discovery.Seedable.ReflectionBased;
using NSeed.Discovery.Seedable;

namespace NSeed.Tests.Unit.Discovery.Seedable.ReflectionBased
{
    // TODO-IG: Extend with the tests for Scenarios.
    public class ReflectionBasedExplicitlyRequiredSeedableDiscovererﾠDiscoverIn
    {
        private readonly IExplicitlyRequiredSeedableDiscoverer<Type, Type> discoverer = new ReflectionBasedExplicitlyRequiredSeedableDiscoverer();

        [Fact]
        public void ReturnsﾠemptyﾠcollectionﾠwhenﾠseedﾠdoesﾠnotﾠhaveﾠRequiresﾠattribute()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void ReturnsﾠemptyﾠcollectionﾠwhenﾠseedﾠhasﾠRequiresﾠattributeﾠwithﾠnullﾠtype()
        {
            Type type = typeof(SeedThatRequiresNull);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }
        [Requires(null)]
        private class SeedThatRequiresNull : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠemptyﾠcollectionﾠwhenﾠseedﾠhasﾠRequiresﾠattributeﾠwithﾠnonﾠseedableﾠtype()
        {
            Type type = typeof(SeedThatRequiresNonSeedableType);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }
        [Requires(typeof(string))]
        private class SeedThatRequiresNonSeedableType : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠRequiresﾠattributeﾠwithﾠseedableﾠtype()
        {
            Type type = typeof(SeedThatRequiresSeedableType);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(SeedThatRequiresNull));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        private class SeedThatRequiresSeedableType : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠmultipleﾠtypesﾠwhenﾠseedﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠseedableﾠtypes()
        {
            Type type = typeof(SeedThatRequiresMultipleSeedableTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(SeedThatRequiresNull), typeof(SeedThatRequiresNonSeedableType), typeof(SeedThatRequiresSeedableType));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresSeedableType))]
        private class SeedThatRequiresMultipleSeedableTypes : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠsameﾠseedableﾠtypes()
        {
            Type type = typeof(SeedThatRequiresSameSeedableTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(SeedThatRequiresNull));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        private class SeedThatRequiresSameSeedableTypes : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠdistinctﾠtypesﾠwhenﾠseedﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠoverlappingﾠseedableﾠtypes()
        {
            Type type = typeof(SeedThatRequiresOverlappingSeedableTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(SeedThatRequiresNull), typeof(SeedThatRequiresNonSeedableType), typeof(SeedThatRequiresSeedableType));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresSeedableType))]
        [Requires(typeof(SeedThatRequiresSeedableType))]
        [Requires(typeof(SeedThatRequiresSeedableType))]
        private class SeedThatRequiresOverlappingSeedableTypes : BaseTestSeed { }
    }
}