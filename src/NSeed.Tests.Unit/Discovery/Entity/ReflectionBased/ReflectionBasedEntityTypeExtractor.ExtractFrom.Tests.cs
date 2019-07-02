using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery.Entity.ReflectionBased;
using NSeed.Discovery.Entity;
using static NSeed.Tests.Unit.Discovery.CommonReflectionBasedExtractorﾠExtractFromﾠTests;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityTypeExtractorﾠExtractFromﾠTests
    {
        private readonly IEntityTypeExtractor<Type> extractor = new ReflectionBasedEntityTypeExtractor();

        [Fact]
        public void Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull()
        {
            Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull<ReflectionBasedEntityTypeExtractor, Type>("entity");
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠconcreteﾠreferenceﾠtype()
        {
            Type type = typeof(string);

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠconcreteﾠvalueﾠtype()
        {
            Type type = typeof(int);

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠtypeﾠwhenﾠtypeﾠisﾠinterface()
        {
            Type type = typeof(ICloneable);

            extractor.ExtractFrom(type).Should().Be(type);
        }

        [Fact]
        public void Shouldﾠextractﾠnullﾠwhenﾠtypeﾠisﾠgenericﾠtypeﾠparameter()
        {
            Type type = GetGenericTypeParameter();
            type.Should().Match(it => it.IsGenericParameter);

            extractor.ExtractFrom(type).Should().BeNull();

            Type GetGenericTypeParameter()
            {
                return typeof(ClassWithGenericMethod)
                    .GetMethod("Method")
                    .ReturnType;
            }
        }

        private class ClassWithGenericMethod
        {
            public T Method<T>(T t) => t;
        }
    }
}