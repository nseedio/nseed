using FluentAssertions;
using NSeed.Extensions;
using NSeed.Tests.Unit.Discovery.Seedable;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Extensions
{
    public class TypeExtensionsﾠIsYieldType
    {
        [Fact]
        public void Returnsﾠtrueﾠwhenﾠtypeﾠisﾠyieldﾠtype()
        {
            Type type = typeof(SeedWithYield.Yield);

            type.IsYieldType().Should().BeTrue();
        }
        private class SeedWithYield : BaseTestSeed { public class Yield : YieldOf<SeedWithYield> { } }

        [Fact]
        public void ReturnsﾠfalseﾠwhenﾠtypeﾠisﾠnotﾠnamedﾠYield()
        {
            Type type = typeof(SeedWithYieldNotNamedYield.NotYield);

            type.IsYieldType().Should().BeFalse();
        }
        private class SeedWithYieldNotNamedYield : BaseTestSeed { public class NotYield : YieldOf<SeedWithYield> { } }

        [Fact]
        public void ReturnsﾠfalseﾠwhenﾠtypeﾠisﾠnotﾠderivedﾠfromﾠYieldOf()
        {
            Type type = typeof(SeedWithYieldNotDerivedFromYieldOf.Yield);

            type.IsYieldType().Should().BeFalse();
        }
        private class SeedWithYieldNotDerivedFromYieldOf : BaseTestSeed { public class Yield : BaseTestSeed<SeedWithYieldNotDerivedFromYieldOf> { } }

        [Fact]
        public void Returnsﾠfalseﾠwhenﾠtypeﾠisﾠnotﾠaﾠyieldﾠofﾠitsﾠownﾠseed()
        {
            Type type = typeof(SeedWithYieldOfAnotherSeed.Yield);

            type.IsYieldType().Should().BeFalse();
        }
        private class SeedWithYieldOfAnotherSeed : BaseTestSeed { public class Yield : YieldOf<SeedWithYield> { } }

        [Fact]
        public void Returnsﾠfalseﾠwhenﾠtypeﾠisﾠnotﾠnested()
        {
            Type type = typeof(Yield);

            type.IsYieldType().Should().BeFalse();
        }
        private class Yield : YieldOf<SeedWithYield> { }
    }
}
