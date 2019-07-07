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
    public class ReflectionBasedExplicitlyRequiredSeedableDiscovererﾠDiscoverInﾠTests
    {
        private readonly IExplicitlyRequiredSeedableDiscoverer<Type, Type> discoverer = new ReflectionBasedExplicitlyRequiredSeedableDiscoverer();

        [Fact]
        public void ShouldﾠreturnﾠemptyﾠcollectionﾠwhenﾠseedﾠdoesﾠnotﾠhaveﾠRequiresﾠattribute()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void ShouldﾠreturnﾠemptyﾠcollectionﾠwhenﾠseedﾠhasﾠRequiresﾠattributeﾠwithﾠnullﾠtype()
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
        public void ShouldﾠreturnﾠemptyﾠcollectionﾠwhenﾠseedﾠhasﾠRequiresﾠattributeﾠwithﾠnonﾠseedableﾠtype()
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
        public void ShouldﾠreturnﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠRequiresﾠattributeﾠwithﾠseedableﾠtype()
        {
            Type type = typeof(SeedThatRequiresSeedableType);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(SeedThatRequiresNull));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        private class SeedThatRequiresSeedableType : BaseTestSeed { }

        [Fact]
        public void ShouldﾠreturnﾠmultipleﾠtypesﾠwhenﾠseedﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠseedableﾠtypes()
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
        public void ShouldﾠreturnﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠsameﾠseedableﾠtypes()
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
        public void ShouldﾠreturnﾠdistinctﾠtypesﾠwhenﾠseedﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠoverlappingﾠseedableﾠtypes()
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