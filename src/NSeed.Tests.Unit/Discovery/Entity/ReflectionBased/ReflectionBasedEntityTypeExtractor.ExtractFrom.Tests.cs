using FluentAssertions;
using NSeed.Discovery.Entity;
using NSeed.Discovery.Entity.ReflectionBased;
using NSeed.MetaInfo;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityTypeExtractorﾠExtractFrom
    {
        private readonly IEntityTypeExtractor<Type> extractor = new ReflectionBasedEntityTypeExtractor();

        [Fact]
        public void Extractsﾠtypeﾠwhenﾠtypeﾠisﾠconcreteﾠreferenceﾠtype()
        {
            Type type = typeof(string);

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Extractsﾠtypeﾠwhenﾠtypeﾠisﾠconcreteﾠvalueﾠtype()
        {
            Type type = typeof(int);

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Extractsﾠtypeﾠwhenﾠtypeﾠisﾠinterface()
        {
            Type type = typeof(ICloneable);

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Extractsﾠnullﾠwhenﾠtypeﾠisﾠgenericﾠtypeﾠparameter()
        {
            Type type = TestHelper.GetGenericTypeParameter();

            extractor.ExtractFrom(type).Should().BeNull();
        }
    }
}
