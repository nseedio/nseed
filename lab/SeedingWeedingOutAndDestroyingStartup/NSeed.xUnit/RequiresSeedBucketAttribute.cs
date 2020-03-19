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
    public sealed class RequiresSeedBucketAttribute : BeforeAfterTestAttribute
    {
        private readonly Type seedBucketType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresSeedBucketAttribute"/> class.
        /// </summary>
        /// <param name="seedBucketType">TODO.</param>
        public RequiresSeedBucketAttribute(Type seedBucketType)
        {
            // TODO: Check not null, must be seedbucket.

            this.seedBucketType = seedBucketType;
        }

        /// <inheritdoc/>
        public override void Before(MethodInfo methodUnderTest)
        {
            base.Before(methodUnderTest);

            // It is not possible to come to the instance of the ITestOutputHelper.
            // This means, all the output from the seeding will not be provided within the test output.

            // TODO: Find a Startup within the test assembly.

            var outputSink = new InternalQueueOutputSink();
            var seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

            var seeder = new Seeder(seedBucketInfoBuilder, outputSink);

            var seedingReport = seeder.Seed(seedBucketType).Result;

            // TODO: See the best way to get the error message, inner exception and those kind of things. Throw some rich exception accordingly e.g. SeedingSingleSeedFaildException.
            if (seedingReport.Status != SeedingStatus.Succeeded) throw new Exception($"Seeding failed.{Environment.NewLine}{outputSink.GetOutputAsString()}");
        }
    }
}
