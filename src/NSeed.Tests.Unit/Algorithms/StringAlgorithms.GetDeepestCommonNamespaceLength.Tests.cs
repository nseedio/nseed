using FluentAssertions;
using NSeed.Algorithms;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NSeed.Tests.Unit.Algorithms
{
    public class StringAlgorithmsﾠGetDeepestCommonNamespaceLength
    {
        [Fact]
        public void Returnsﾠzeroﾠwhenﾠtypeﾠnameﾠlistﾠisﾠempty()
        {
            var typeNames = Array.Empty<string>();

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(0);
        }

        [Fact]
        public void Returnsﾠzeroﾠwhenﾠoneﾠtypeﾠnameﾠisﾠempty()
        {
            var typeNames = new[]
            {
                string.Empty
            };

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(0);
        }

        [Fact]
        public void Returnsﾠzeroﾠwhenﾠoneﾠofﾠtypeﾠnamesﾠisﾠempty()
        {
            var typeNames = new[]
            {
                string.Empty,
                "A.SomeName"
            };

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(0);
        }

        [Fact]
        public void Returnsﾠzeroﾠwhenﾠoneﾠofﾠtypeﾠnamesﾠisﾠwhitespace()
        {
            var typeNames = new[]
            {
                "  ",
                "A.SomeName"
            };

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(0);
        }

        [Fact]
        public void Returnsﾠzeroﾠwhenﾠsameﾠtypeﾠnamesﾠdoﾠnotﾠhaveﾠnamespaces()
        {
            var typeNames = new[]
            {
                "SomeName",
                "SomeName"
            };

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(0);
        }

        [Fact]
        public void Returnsﾠzeroﾠwhenﾠsameﾠtypeﾠnamesﾠhaveﾠdifferentﾠnamespaces()
        {
            var typeNames = new[]
            {
                "A.SomeName",
                "B.SomeName"
            };

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(0);
        }

        [Fact]
        public void Returnsﾠzeroﾠwhenﾠdifferentﾠtypeﾠnamesﾠdoﾠnotﾠhaveﾠnamespaces()
        {
            var typeNames = new[]
            {
                "SomeName",
                "SomeOtherName"
            };

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(0);
        }

        [Fact]
        public void Returnsﾠzeroﾠwhenﾠdifferentﾠtypeﾠnamesﾠhaveﾠdifferentﾠnamespaces()
        {
            var typeNames = new[]
            {
                "A.SomeName",
                "B.SomeOtherName"
            };

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(0);
        }

        [Fact]
        public void Returnsﾠoneﾠwhenﾠtypeﾠnamesﾠstartﾠwithﾠdot() // Not a valid programming case.
        {
            var typeNames = new[]
            {
                ".SomeName",
                ".SomeOtherName"
            };

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(1);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("A.B")]
        [InlineData("A.B.C")]
        public void Returnsﾠexpectedﾠlengthﾠwhenﾠtypeﾠnamesﾠstartﾠwithﾠcommonﾠnamespace(string @namespace)
        {
            var typeNames = new[]
            {
                $"{@namespace}.SomeName",
                $"{@namespace}.SomeName",
                $"{@namespace}.SomeName.Something",
                $"{@namespace}.SomeOtherName",
                $"{@namespace}.SomeOtherName.Something",
                $"{@namespace}.SomeOtherName"
            };

            var length = StringAlgorithms.GetDeepestCommonNamespaceLength(typeNames);

            length.Should().Be(@namespace.Length + 1);
        }
    }
}
