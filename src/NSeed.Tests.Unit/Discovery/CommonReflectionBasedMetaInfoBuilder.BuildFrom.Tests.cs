using System;
using FluentAssertions;
using NSeed.Discovery;
using NSeed.MetaInfo;

namespace NSeed.Tests.Unit.Discovery
{
    internal static class CommonReflectionBasedMetaInfoBuilderﾠBuildFrom
    {
        // See comment in CommonReflectionBasedExtractorﾠExtractFromﾠTests.
        internal static void Shouldﾠthrowﾠexceptionﾠwhenﾠimplementationﾠtypeﾠisﾠnull<TMetaInfoBuilder, TMetaInfo>()
            where TMetaInfoBuilder : class, IMetaInfoBuilder<Type, TMetaInfo>, new()
            where TMetaInfo: MetaInfo.MetaInfo
        {
            new TMetaInfoBuilder().Invoking(x => x.BuildFrom(null))
                .Should()
                .Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("implementation");
        }
    }
}