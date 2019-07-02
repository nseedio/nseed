using System;
using FluentAssertions;
using NSeed.Discovery;
using NSeed.MetaInfo;

namespace NSeed.Tests.Unit.Discovery
{
    internal static class CommonReflectionBasedMetaInfoBuilderﾠBuildFromﾠTests
    {
        // See comment in CommonReflectionBasedExtractorﾠExtractFromﾠTests.
        internal static void Shouldﾠthrowﾠexceptionﾠwhenﾠimplementationﾠtypeﾠisﾠnull<TMetaInfoBuilder, TMetaInfo>()
            where TMetaInfoBuilder : class, IMetaInfoBuilder<Type, TMetaInfo>, new()
            where TMetaInfo: BaseMetaInfo
        {
            new TMetaInfoBuilder().Invoking(x => x.BuildFrom(null))
                .Should()
                .Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("implementation");
        }
    }
}