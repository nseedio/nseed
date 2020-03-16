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
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class RequiresAttribute : BeforeAfterTestAttribute
    {
        private readonly Type seedableType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresAttribute"/> class.
        /// </summary>
        /// <param name="seedableType">TODO.</param>
        public RequiresAttribute(Type seedableType) // TODO: Params.
        {
            // TODO: Check not null, must be seed, or scenario, or seedbucket.

            // TODO: During the execution do all the possible combinations: seedables from the same seed bucket, seed bucket only, several seed buckets, combinations...

            this.seedableType = seedableType;
        }

        /// <inheritdoc/>
        public override void Before(MethodInfo methodUnderTest)
        {
            base.Before(methodUnderTest);

            // TODO: So far seedable type can only be a seed bucket.
            // TODO: Find out how to get ITestOutputHelper instance to write to. Brute force: scan the defining type of the methodUnderTest.
            // TODO: Find a Startup within the test assembly.

            var outputSink = new VoidOutputSink();
            var seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

            var seeder = new Seeder(seedBucketInfoBuilder, outputSink);
            seeder.Seed(seedableType);
        }
    }
}
