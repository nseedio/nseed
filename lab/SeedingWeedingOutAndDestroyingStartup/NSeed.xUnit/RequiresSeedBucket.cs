using System;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NSeed.Xunit
{
    /// <summary>
    /// TODO.
    /// </summary>
    /// <typeparam name="TSeedBucket">TODO.TODO.</typeparam>
    public class RequiresSeedBucket<TSeedBucket> : RequiresSeedBucketFixture
        where TSeedBucket : SeedBucket
    {
        public RequiresSeedBucket()
            : base(typeof(TSeedBucket), null)
        {
        }
    }

    /// <summary>
    /// TODO.
    /// </summary>
    /// <typeparam name="TSeedBucket">TODO.TODO.</typeparam>
    /// <typeparam name="TSeedingStartupType">TODO.TODO.TODO.</typeparam>
#pragma warning disable SA1402 // File may only contain a single type
    public class RequiresSeedBucket<TSeedBucket, TSeedingStartupType> : RequiresSeedBucketFixture
#pragma warning restore SA1402 // File may only contain a single type
        where TSeedBucket : SeedBucket
        where TSeedingStartupType : SeedBucketStartup
    {
        public RequiresSeedBucket()
            : base(typeof(TSeedBucket), typeof(TSeedingStartupType))
        {
        }
    }
}
