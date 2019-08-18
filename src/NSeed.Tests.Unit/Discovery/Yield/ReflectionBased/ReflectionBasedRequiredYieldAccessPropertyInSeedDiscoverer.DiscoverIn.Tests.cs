using FluentAssertions;
using Moq;
using NSeed.Discovery.Yield;
using NSeed.Discovery.Yield.ReflectionBased;
using NSeed.Tests.Unit.Discovery.Seedable;
using System;
using System.Reflection;
using Xunit;

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
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
            public Yield MyOwnYield { get; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
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
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
            public SeedA.Yield YieldOfA { get; }
            internal SeedB.Yield YieldOfB { get; }
            protected SeedD.Yield YieldOfD { get; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used in tests via reflection.")]
            private SeedC.Yield YieldOfC { get; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
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
            public static SeedA.Yield YieldOfA { get; }
            internal static SeedB.Yield YieldOfB { get; }
            protected static SeedD.Yield YieldOfD { get; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used in tests via reflection.")]
            private static SeedC.Yield YieldOfC { get; }
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
