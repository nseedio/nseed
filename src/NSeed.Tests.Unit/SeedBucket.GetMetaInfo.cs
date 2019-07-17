using Xunit;
using NSeed.Extensions;
using FluentAssertions;
using NSeed.MetaInfo;

namespace NSeed.Tests.Unit
{
    public class SeedBucketﾠGetMetaInfo
    {
        private SeedAssemblyBuilder assemblyBuilder = new SeedAssemblyBuilder();

        [Fact]
        public void ReturnsﾠexpectedﾠminimalﾠSeedBucketInfo()
        {
            var seedBucket = assemblyBuilder.AddSeedBucket().BuildAndGetSeedBucket();
            var type = seedBucket.GetType();

            var expected = new SeedBucketInfo
            (
                type,
                type.FullName,
                type.Name.Humanize(),
                string.Empty
            );

            seedBucket.GetMetaInfo().Should().BeEquivalentTo(expected);
        }
    }
}