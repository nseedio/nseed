using System;
using FluentAssertions;
using NSeed.Discovery.Seed;
using NSeed.Guards;

namespace NSeed.Tests.Unit.Discovery.Seed.ReflectionBased
{
    internal static class CommonReflectionBasedSeedExtractorﾠExtractFromﾠTests
    {
        // Unfortunately, this was the only way to reuse at least the implementation of tests
        // common to all Seed extractors. The best way to do it would be to have a parametrized
        // based test class that would be parameterized same in the same way as these methods
        // and provide a protected extractor object to be used in derived test classes.
        // But, unfortunately, xUnit test classes must be public :-(
        // That means, to implement the solution mentioned above, we should make the *internal* IExtractor<,>
        // interface public. Of course, we do not want to make IExtractor<,> public.
        // That's why this rather primitive workaround to at least reuse the implementation.
        internal static void Shouldﾠthrowﾠinternalﾠerrorﾠwhenﾠtypeﾠisﾠnull<TExtractor, TExtract>()
            where TExtractor : class, IExtractor<Type, TExtract>, new()
        {
            new TExtractor().Invoking(x => x.ExtractFrom(null))
                .Should()
                .Throw<NSeedInternalErrorArgumentNullException>()
                .And.ParamName.Should().Be("seedImplementation");
        }
    }
}