using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery.Entity.ReflectionBased;
using NSeed.Discovery.Entity;
using static NSeed.Tests.Unit.Discovery.CommonReflectionBasedExtractorﾠExtractFromﾠTests;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityFullNameExtractorﾠExtractFromﾠTests
    {
        private readonly IEntityFullNameExtractor<Type> extractor = new ReflectionBasedEntityFullNameExtractor();

        [Fact]
        public void Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull()
        {
            Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull<ReflectionBasedEntityFullNameExtractor, string>("entity");
        }

        [Fact]
        public void Shouldﾠextractﾠfullﾠnameﾠwhenﾠtypeﾠisﾠconcreteﾠreferenceﾠtype()
        {
            Type type = typeof(string);

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Shouldﾠextractﾠfullﾠnameﾠwhenﾠtypeﾠisﾠconcreteﾠvalueﾠtype()
        {
            Type type = typeof(int);

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Shouldﾠextractﾠfullﾠnameﾠwhenﾠtypeﾠisﾠinterface()
        {
            Type type = typeof(ICloneable);

            extractor.ExtractFrom(type).Should().Be(type.FullName);
        }

        [Fact]
        public void Shouldﾠextractﾠemptyﾠstringﾠwhenﾠtypeﾠisﾠgenericﾠtypeﾠparameter()
        {
            Type type = TestHelper.GetGenericTypeParameter();

            extractor.ExtractFrom(type).Should().BeEmpty();
        }
    }
}