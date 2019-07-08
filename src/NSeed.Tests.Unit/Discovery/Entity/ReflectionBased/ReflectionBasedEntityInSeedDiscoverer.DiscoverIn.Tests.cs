using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSeed.Discovery.Entity.ReflectionBased;
using NSeed.Discovery.Entity;
using NSeed.Tests.Unit.Discovery.Seedable;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityInSeedDiscovererﾠDiscoverIn
    {
        private readonly IEntityInSeedDiscoverer<Type, Type> discoverer = new ReflectionBasedEntityInSeedDiscoverer();

        [Fact]
        public void Returnsﾠemptyﾠcollectionﾠwhenﾠseedﾠdoesﾠnotﾠhaveﾠentities()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Returnsﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠsingleﾠreferenceﾠentityﾠtype()
        {
            Type type = new Mock<ISeed<string>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string));
        }

        [Fact]
        public void Returnsﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠsingleﾠvalueﾠentityﾠtype()
        {
            Type type = new Mock<ISeed<int>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(int));
        }

        [Fact]
        public void Returnsﾠtwoﾠtypesﾠwhenﾠseedﾠhasﾠtwoﾠentityﾠtypes()
        {
            Type type = new Mock<ISeed<string, int>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string), typeof(int));
        }

        [Fact]
        public void Returnsﾠthreeﾠtypesﾠwhenﾠseedﾠhasﾠthreeﾠentityﾠtypes()
        {
            Type type = new Mock<ISeed<string, int, object>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string), typeof(int), typeof(object));
        }

        [Fact]
        public void Returnsﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠtwoﾠsameﾠentityﾠtypes()
        {
            Type type = new Mock<ISeed<string, string>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string));
        }

        [Fact]
        public void Returnsﾠdistinctﾠtypesﾠwhenﾠseedﾠhasﾠoverlappingﾠentityﾠtypes()
        {
            Type type = new Mock<ISeed<string, string, int>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string), typeof(int));
        }

        [Fact]
        public void ReturnsﾠallﾠtypesﾠwhenﾠseedﾠimplementsﾠseveralﾠISeedﾠinterfacesﾠwithﾠdistinctﾠentityﾠtypes()
        {
            Type type = typeof(SeedThatImplementsSeveralISeedInterfacesWithDistinctEntityTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(object), typeof(string), typeof(int), typeof(double), typeof(float), typeof(decimal));
        }

        private class SeedThatImplementsSeveralISeedInterfacesWithDistinctEntityTypes
            : BaseTestSeed, ISeed<object>, ISeed<string, int>, ISeed<double, float, decimal>
        { }

        [Fact]
        public void ReturnsﾠdistinctﾠtypesﾠwhenﾠseedﾠimplementsﾠseveralﾠISeedﾠinterfacesﾠwithﾠoverlappingﾠentityﾠtypes()
        {
            Type type = typeof(SeedThatImplementsSeveralISeedInterfacesWithOverlappingEntityTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(object), typeof(int));
        }

        private class SeedThatImplementsSeveralISeedInterfacesWithOverlappingEntityTypes
            : BaseTestSeed, ISeed<object>, ISeed<object, int>, ISeed<int, object, int>
        { }
    }
}