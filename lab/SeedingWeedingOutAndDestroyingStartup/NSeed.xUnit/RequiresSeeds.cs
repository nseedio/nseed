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
    /// <typeparam name="TSeed">TODO.TODO.</typeparam>
    /// <typeparam name="TSeedingStartup">TODO--.</typeparam>
    public class RequiresSeeds<TSeed, TSeedingStartup> : RequiresSeedsFixture
        where TSeed : ISeed
        where TSeedingStartup : SeedBucketStartup
    {
        public RequiresSeeds()
            : base(typeof(TSeedingStartup), typeof(TSeed))
        {
        }
    }

    /// <summary>
    /// TODO.
    /// </summary>
    /// <typeparam name="TSeed1">TODO.TODO.</typeparam>
    /// <typeparam name="TSeed2">TODO.TODO.TODO.</typeparam>
    /// <typeparam name="TSeedingStartup">TODO--.</typeparam>
#pragma warning disable SA1402 // File may only contain a single type
    public class RequiresSeeds<TSeed1, TSeed2, TSeedingStartup> : RequiresSeedsFixture
#pragma warning restore SA1402 // File may only contain a single type
        where TSeed1 : ISeed
        where TSeed2 : ISeed
        where TSeedingStartup : SeedBucketStartup
    {
        public RequiresSeeds()
            : base(typeof(TSeedingStartup), typeof(TSeed1), typeof(TSeed2))
        {
        }
    }

    /// <summary>
    /// TODO.
    /// </summary>
    /// <typeparam name="TSeed1">TODO.TODO.</typeparam>
    /// <typeparam name="TSeed2">TODO.TODO.TODO.</typeparam>
    /// <typeparam name="TSeed3">TODO----.</typeparam>
    /// <typeparam name="TSeedingStartup">TODO--.</typeparam>
#pragma warning disable SA1402 // File may only contain a single type
    public class RequiresSeeds<TSeed1, TSeed2, TSeed3, TSeedingStartup> : RequiresSeedsFixture
#pragma warning restore SA1402 // File may only contain a single type
        where TSeed1 : ISeed
        where TSeed2 : ISeed
        where TSeed3 : ISeed
        where TSeedingStartup : SeedBucketStartup
    {
        public RequiresSeeds()
            : base(typeof(TSeedingStartup), typeof(TSeed1), typeof(TSeed2), typeof(TSeed3))
        {
        }
    }
}
