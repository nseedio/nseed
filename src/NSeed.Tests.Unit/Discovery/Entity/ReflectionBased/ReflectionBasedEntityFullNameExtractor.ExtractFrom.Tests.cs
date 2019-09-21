using FluentAssertions;
using NSeed.Discovery.Entity;
using NSeed.Discovery.Entity.ReflectionBased;
using NSeed.MetaInfo;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityFullNameExtractorﾠExtractFrom
    {
        private readonly IEntityFullNameExtractor<Type> extractor = new ReflectionBasedEntityFullNameExtractor();

        [Fact]
        public void Extractsﾠfullﾠnameﾠwhenﾠtypeﾠisﾠconcreteﾠreferenceﾠtype()
        {
            Type type = typeof(string);

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠfullﾠnameﾠwhenﾠtypeﾠisﾠconcreteﾠvalueﾠtype()
        {
            Type type = typeof(int);

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠfullﾠnameﾠwhenﾠtypeﾠisﾠinterface()
        {
            Type type = typeof(ICloneable);

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Extractsﾠemptyﾠstringﾠwhenﾠtypeﾠisﾠgenericﾠtypeﾠparameter()
        {
            Type type = TestHelper.GetGenericTypeParameter();

            extractor.ExtractFrom(type).Should().BeEmpty();
        }
    }
}
