using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery.Entity.ReflectionBased;
using NSeed.Discovery.Entity;
using static NSeed.Tests.Unit.Discovery.CommonReflectionBasedExtractorﾠExtractFromﾠTests;
using NSeed.MetaInfo;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityTypeExtractorﾠExtractFromﾠTests
    {
        private readonly IEntityTypeExtractor<Type> extractor = new ReflectionBasedEntityTypeExtractor();
        private readonly DistinctErrorCollectorAndProvider collector = new DistinctErrorCollectorAndProvider();

        [Fact]
        public void Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull()
        {
            Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull<ReflectionBasedEntityTypeExtractor, Type>("entity");
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠconcreteﾠreferenceﾠtype()
        {
            Type type = typeof(string);

            extractor.ExtractFrom(type, collector).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠconcreteﾠvalueﾠtype()
        {
            Type type = typeof(int);

            extractor.ExtractFrom(type, collector).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠinterface()
        {
            Type type = typeof(ICloneable);

            extractor.ExtractFrom(type, collector).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠnullﾠwhenﾠtypeﾠisﾠgenericﾠtypeﾠparameter()
        {
            Type type = TestHelper.GetGenericTypeParameter();

            extractor.ExtractFrom(type, collector).Should().BeNull();
        }
    }
}