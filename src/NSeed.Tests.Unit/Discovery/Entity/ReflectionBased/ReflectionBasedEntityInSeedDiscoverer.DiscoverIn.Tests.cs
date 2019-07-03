using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSeed.Discovery.Entity.ReflectionBased;
using NSeed.Tests.Unit.Discovery.Seed;
using NSeed.Discovery.Entity;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityInSeedDiscovererﾠDiscoverInﾠTests
    {
        private readonly IEntityInSeedDiscoverer<Type, Type> discoverer = new ReflectionBasedEntityInSeedDiscoverer();

        [Fact]
        public void Shouldﾠreturnﾠemptyﾠcollectionﾠwhenﾠseedﾠdoesﾠnotﾠhaveﾠentities()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Shouldﾠreturnﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠsingleﾠreferenceﾠentityﾠtype()
        {
            Type type = new Mock<ISeed<string>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string));
        }

        [Fact]
        public void Shouldﾠreturnﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠsingleﾠvalueﾠentityﾠtype()
        {
            Type type = new Mock<ISeed<int>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(int));
        }

        [Fact]
        public void Shouldﾠreturnﾠtwoﾠtypesﾠwhenﾠseedﾠhasﾠtwoﾠentityﾠtypes()
        {
            Type type = new Mock<ISeed<string, int>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string), typeof(int));
        }

        [Fact]
        public void Shouldﾠreturnﾠthreeﾠtypesﾠwhenﾠseedﾠhasﾠthreeﾠentityﾠtypes()
        {
            Type type = new Mock<ISeed<string, int, object>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string), typeof(int), typeof(object));
        }

        [Fact]
        public void Shouldﾠreturnﾠsingleﾠtypeﾠwhenﾠseedﾠhasﾠtwoﾠsameﾠentityﾠtypes()
        {
            Type type = new Mock<ISeed<string, string>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string));
        }

        [Fact]
        public void Shouldﾠreturnﾠdistinctﾠtypesﾠwhenﾠseedﾠhasﾠoverlappingﾠentityﾠtypes()
        {
            Type type = new Mock<ISeed<string, string, int>>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(string), typeof(int));
        }

        // This will generate errors, but we will provide the entity types.
        [Fact]
        public void ShouldﾠreturnﾠallﾠtypesﾠwhenﾠseedﾠimplementsﾠseveralﾠISeedﾠinterfacesﾠwithﾠdistinctﾠentityﾠtypes()
        {
            Type type = typeof(SeedThatImplementsSeveralISeedInterfacesWithDistinctEntityTypes);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should().BeEquivalentTo(typeof(object), typeof(string), typeof(int), typeof(double), typeof(float), typeof(decimal));
        }

        private class SeedThatImplementsSeveralISeedInterfacesWithDistinctEntityTypes
            : BaseTestSeed, ISeed<object>, ISeed<string, int>, ISeed<double, float, decimal>
        { }

        // This will generate errors, but we will provide the entity types.
        [Fact]
        public void ShouldﾠreturnﾠdistinctﾠtypesﾠwhenﾠseedﾠimplementsﾠseveralﾠISeedﾠinterfacesﾠwithﾠoverlappingﾠentityﾠtypes()
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