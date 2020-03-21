using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Seeding;
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
    public class RequiresSeedBucket<TSeedBucket>
        where TSeedBucket : SeedBucket
    {
        public RequiresSeedBucket()
            : this(typeof(TSeedBucket))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresSeedBucket{TSeedBucket}"/> class.
        /// </summary>
        /// <param name="seedBucketType">TODO.</param>
        /// <param name="seedingStartupType">TODO.TODO.</param>
        protected RequiresSeedBucket(Type seedBucketType)
        {
           RequiresSeedBucketHelper.SeedSeedBucket(seedBucketType, null);
        }
    }

    /// <summary>
    /// TODO.
    /// </summary>
    /// <typeparam name="TSeedBucket">TODO.TODO.</typeparam>
    /// <typeparam name="TSeedingStartupType">TODO.TODO.TODO.</typeparam>
#pragma warning disable SA1402 // File may only contain a single type
    public class RequiresSeedBucket<TSeedBucket, TSeedingStartupType>
#pragma warning restore SA1402 // File may only contain a single type
        where TSeedBucket : SeedBucket
        where TSeedingStartupType : SeedBucketStartup
    {
        public RequiresSeedBucket()
            : this(typeof(TSeedBucket), typeof(TSeedingStartupType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresSeedBucket{TSeedBucket, TSeedingStartupType}"/> class.
        /// </summary>
        /// <param name="seedBucketType">TODO.</param>
        /// <param name="seedingStartupType">TODO.TODO.</param>
        protected RequiresSeedBucket(Type seedBucketType, Type seedingStartupType)
        {
            RequiresSeedBucketHelper.SeedSeedBucket(seedBucketType, seedingStartupType);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    internal static class RequiresSeedBucketHelper
#pragma warning restore SA1402 // File may only contain a single type
    {
        internal static void SeedSeedBucket(Type seedBucketType, Type? seedingStartupType)
        {
            // TODO: Check not null, must be seedbucket.

            var outputSink = new InternalQueueOutputSink();
            var seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

            var seeder = new Seeder(seedBucketInfoBuilder, outputSink);

            // TODO: Is not null, IsSeedBucketSeedingType etc.

            var seedingReport = seedingStartupType is null
                ? seeder.SeedSeedBucket(seedBucketType).Result
                : seeder.SeedSeedBucket(seedBucketType, seedingStartupType).Result;

            // TODO: See the best way to get the error message, inner exception and those kind of things. Throw some rich exception accordingly e.g. SeedingSingleSeedFaildException.
            if (seedingReport.Status != SeedingStatus.Succeeded) throw new Exception($"Seeding failed.{Environment.NewLine}{outputSink.GetOutputAsString()}");
        }
    }
}
