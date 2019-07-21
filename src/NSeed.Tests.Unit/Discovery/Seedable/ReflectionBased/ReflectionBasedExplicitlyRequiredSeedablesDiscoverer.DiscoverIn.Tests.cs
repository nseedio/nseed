using FluentAssertions;
using Moq;
using NSeed.Discovery.Seedable;
using NSeed.Discovery.Seedable.ReflectionBased;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Discovery.Seedable.ReflectionBased
{
    public class ReflectionBasedExplicitlyRequiredSeedablesDiscovererﾠDiscoverIn
    {
        private readonly IExplicitlyRequiredSeedablesDiscoverer<Type, Type> discoverer = new ReflectionBasedExplicitlyRequiredSeedablesDiscoverer();

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
        public void ReturnsﾠemptyﾠcollectionﾠwhenﾠscenarioﾠdoesﾠnotﾠhaveﾠRequiresﾠattribute()
        {
            Type type = new Mock<IScenario>().Object.GetType();

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
        public void ReturnsﾠemptyﾠcollectionﾠwhenﾠscenarioﾠhasﾠRequiresﾠattributeﾠwithﾠnullﾠtype()
        {
            Type type = typeof(ScenarioThatRequiresNull);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }
        [Requires(null)]
        private class ScenarioThatRequiresNull : BaseTestScenario { }

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
        public void ReturnsﾠemptyﾠcollectionﾠwhenﾠscenarioﾠhasﾠRequiresﾠattributeﾠwithﾠnonﾠseedableﾠtype()
        {
            Type type = typeof(ScenarioThatRequiresNonSeedableType);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }
        [Requires(typeof(string))]
        private class ScenarioThatRequiresNonSeedableType : BaseTestScenario { }

        [Fact]
        public void ReturnsﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠRequiresﾠattributeﾠwithﾠseedﾠtype()
        {
            Type type = typeof(SeedThatRequiresSeedType);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(SeedThatRequiresNull));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        private class SeedThatRequiresSeedType : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠsingleﾠtypeﾠwhenﾠscenarioﾠhasﾠRequiresﾠattributeﾠwithﾠseedﾠtype()
        {
            Type type = typeof(ScenarioThatRequiresSeedType);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(SeedThatRequiresNull));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        private class ScenarioThatRequiresSeedType : BaseTestScenario { }

        [Fact]
        public void ReturnsﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠRequiresﾠattributeﾠwithﾠscenarioﾠtype()
        {
            Type type = typeof(SeedThatRequiresScenarioType);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(ScenarioThatRequiresNull));
        }
        [Requires(typeof(ScenarioThatRequiresNull))]
        private class SeedThatRequiresScenarioType : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠsingleﾠtypeﾠwhenﾠscenarioﾠhasﾠRequiresﾠattributeﾠwithﾠscenarioﾠtype()
        {
            Type type = typeof(ScenarioThatRequiresScenarioType);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(ScenarioThatRequiresNull));
        }
        [Requires(typeof(ScenarioThatRequiresNull))]
        private class ScenarioThatRequiresScenarioType : BaseTestScenario { }

        [Fact]
        public void ReturnsﾠmultipleﾠtypesﾠwhenﾠseedﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠseedableﾠtypes()
        {
            Type type = typeof(SeedThatRequiresMultipleSeedableTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(
                    typeof(SeedThatRequiresNull), typeof(SeedThatRequiresNonSeedableType), typeof(SeedThatRequiresSeedType),
                    typeof(ScenarioThatRequiresNull), typeof(ScenarioThatRequiresNonSeedableType), typeof(ScenarioThatRequiresSeedType));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresSeedType))]
        [Requires(typeof(ScenarioThatRequiresNull))]
        [Requires(typeof(ScenarioThatRequiresNonSeedableType))]
        [Requires(typeof(ScenarioThatRequiresSeedType))]
        private class SeedThatRequiresMultipleSeedableTypes : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠmultipleﾠtypesﾠwhenﾠscenarioﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠseedableﾠtypes()
        {
            Type type = typeof(ScenarioThatRequiresMultipleSeedableTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(
                    typeof(SeedThatRequiresNull), typeof(SeedThatRequiresNonSeedableType), typeof(SeedThatRequiresSeedType),
                    typeof(ScenarioThatRequiresNull), typeof(ScenarioThatRequiresNonSeedableType), typeof(ScenarioThatRequiresSeedType));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresSeedType))]
        [Requires(typeof(ScenarioThatRequiresNull))]
        [Requires(typeof(ScenarioThatRequiresNonSeedableType))]
        [Requires(typeof(ScenarioThatRequiresSeedType))]
        private class ScenarioThatRequiresMultipleSeedableTypes : BaseTestScenario { }

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
        public void ReturnsﾠsingleﾠtypeﾠwhenﾠscenarioﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠsameﾠseedableﾠtypes()
        {
            Type type = typeof(ScenarioThatRequiresSameSeedableTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(SeedThatRequiresNull));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        private class ScenarioThatRequiresSameSeedableTypes : BaseTestScenario { }

        [Fact]
        public void ReturnsﾠdistinctﾠtypesﾠwhenﾠseedﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠoverlappingﾠseedableﾠtypes()
        {
            Type type = typeof(SeedThatRequiresOverlappingSeedableTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(
                    typeof(SeedThatRequiresNull), typeof(SeedThatRequiresNonSeedableType), typeof(SeedThatRequiresSeedType),
                    typeof(ScenarioThatRequiresNull), typeof(ScenarioThatRequiresNonSeedableType), typeof(ScenarioThatRequiresSeedType));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresSeedType))]
        [Requires(typeof(SeedThatRequiresSeedType))]
        [Requires(typeof(SeedThatRequiresSeedType))]
        [Requires(typeof(ScenarioThatRequiresNull))]
        [Requires(typeof(ScenarioThatRequiresNull))]
        [Requires(typeof(ScenarioThatRequiresNull))]
        [Requires(typeof(ScenarioThatRequiresNonSeedableType))]
        [Requires(typeof(ScenarioThatRequiresNonSeedableType))]
        [Requires(typeof(ScenarioThatRequiresNonSeedableType))]
        [Requires(typeof(ScenarioThatRequiresSeedType))]
        [Requires(typeof(ScenarioThatRequiresSeedType))]
        [Requires(typeof(ScenarioThatRequiresSeedType))]
        private class SeedThatRequiresOverlappingSeedableTypes : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠdistinctﾠtypesﾠwhenﾠscenarioﾠhasﾠmultipleﾠRequiresﾠattributesﾠwithﾠoverlappingﾠseedableﾠtypes()
        {
            Type type = typeof(ScenarioThatRequiresOverlappingSeedableTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(
                    typeof(SeedThatRequiresNull), typeof(SeedThatRequiresNonSeedableType), typeof(SeedThatRequiresSeedType),
                    typeof(ScenarioThatRequiresNull), typeof(ScenarioThatRequiresNonSeedableType), typeof(ScenarioThatRequiresSeedType));
        }
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNull))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresNonSeedableType))]
        [Requires(typeof(SeedThatRequiresSeedType))]
        [Requires(typeof(SeedThatRequiresSeedType))]
        [Requires(typeof(SeedThatRequiresSeedType))]
        [Requires(typeof(ScenarioThatRequiresNull))]
        [Requires(typeof(ScenarioThatRequiresNull))]
        [Requires(typeof(ScenarioThatRequiresNull))]
        [Requires(typeof(ScenarioThatRequiresNonSeedableType))]
        [Requires(typeof(ScenarioThatRequiresNonSeedableType))]
        [Requires(typeof(ScenarioThatRequiresNonSeedableType))]
        [Requires(typeof(ScenarioThatRequiresSeedType))]
        [Requires(typeof(ScenarioThatRequiresSeedType))]
        [Requires(typeof(ScenarioThatRequiresSeedType))]
        private class ScenarioThatRequiresOverlappingSeedableTypes : BaseTestScenario { }
    }
}
