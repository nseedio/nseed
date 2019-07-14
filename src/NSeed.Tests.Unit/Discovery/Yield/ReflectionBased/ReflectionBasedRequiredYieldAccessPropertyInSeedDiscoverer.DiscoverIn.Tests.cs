using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSeed.Discovery.Yield.ReflectionBased;
using NSeed.Discovery.Yield;
using System.Reflection;
using NSeed.Tests.Unit.Discovery.Seedable;

namespace NSeed.Tests.Unit.Discovery.Yield.ReflectionBased
{
    public class ReflectionBasedRequiredYieldAccessPropertyInSeedDiscovererﾠDiscoverIn
    {
        private readonly IRequiredYieldAccessPropertyInSeedDiscoverer<Type, PropertyInfo> discoverer = new ReflectionBasedRequiredYieldAccessPropertyInSeedDiscoverer();

        [Fact]
        public void Returnsﾠemptyﾠcollectionﾠwhenﾠseedﾠdoesﾠnotﾠrequireﾠyields()
        {
            Type type = new Mock<ISeed>().Object.GetType();

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Returnsﾠemptyﾠcollectionﾠwhenﾠseedﾠrequiresﾠitsﾠownﾠyield()
        {
            Type type = typeof(SeedThatRequiresItsOwnYield);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEmpty();
        }
        private class SeedThatRequiresItsOwnYield : BaseTestSeed
        {
            public Yield MyOwnYield { get; }
            public class Yield : YieldOf<SeedThatRequiresItsOwnYield> { }
        }

        [Fact]
        public void Returnsﾠexpectedﾠyieldﾠaccessﾠproperties()
        {
            Type type = typeof(SeedThatRequiresYields);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEquivalentTo(
                    type.GetPropertyWithName("YieldOfA"),
                    type.GetPropertyWithName("YieldOfB"),
                    type.GetPropertyWithName("YieldOfC"),
                    type.GetPropertyWithName("YieldOfD"));
        }
        private class SeedThatRequiresYields : BaseTestSeed
        {
            public SeedA.Yield YieldOfA { get; }
            internal SeedB.Yield YieldOfB { get; }
            private SeedC.Yield YieldOfC { get; }
            protected SeedD.Yield YieldOfD { get; }
        }
        private class SeedA : BaseTestSeed { public class Yield : YieldOf<SeedA> { } }
        private class SeedB : BaseTestSeed { internal class Yield : YieldOf<SeedB> { } }
        private class SeedC : BaseTestSeed { public class Yield : YieldOf<SeedC> { } }
        private class SeedD : BaseTestSeed { internal class Yield : YieldOf<SeedD> { } }

        [Fact]
        public void Returnsﾠyieldﾠaccessﾠpropertiesﾠalthoughﾠtheyﾠareﾠstatic()
        {
            Type type = typeof(SeedThatRequiresYieldsViaStaticProperties);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEquivalentTo(
                    type.GetPropertyWithName("YieldOfA"),
                    type.GetPropertyWithName("YieldOfB"),
                    type.GetPropertyWithName("YieldOfC"),
                    type.GetPropertyWithName("YieldOfD"));
        }
        private class SeedThatRequiresYieldsViaStaticProperties : BaseTestSeed
        {
            static public SeedA.Yield YieldOfA { get; }
            static internal SeedB.Yield YieldOfB { get; }
            static private SeedC.Yield YieldOfC { get; }
            static protected SeedD.Yield YieldOfD { get; }
        }

        [Fact]
        public void Returnsﾠnonﾠprivateﾠinheritedﾠyieldﾠaccessﾠproperties()
        {
            Type type = typeof(SeedThatRequiresYieldsViaInheritedProperties);

            discoverer.DiscoverIn(type)
                .DiscoveredItems
                .Should()
                .BeEquivalentTo(
                    type.GetPropertyWithName("YieldOfA"),
                    type.GetPropertyWithName("YieldOfB"),
                    // YieldOfC is the missing private property.
                    type.GetPropertyWithName("YieldOfD"));
        }

        private class SeedThatRequiresYieldsViaInheritedProperties : SeedThatRequiresYields { }
    }
}