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

        public Type? SeedingStartupType { get; set; }

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

            // TODO: Is not null, IsSeedBucketSeedingType etc.
            var seedingStartupType = SeedingStartupType ?? FindSeedingStartupType();

            var seedingReport = seedingStartupType is null
                ? seeder.Seed(seedBucketType).Result
                : seeder.Seed(seedBucketType, seedingStartupType).Result;

            // TODO: See the best way to get the error message, inner exception and those kind of things. Throw some rich exception accordingly e.g. SeedingSingleSeedFaildException.
            if (seedingReport.Status != SeedingStatus.Succeeded) throw new Exception($"Seeding failed.{Environment.NewLine}{outputSink.GetOutputAsString()}");

            Type? FindSeedingStartupType()
            {
                var seedingStartupAttribute = methodUnderTest.GetCustomAttribute<UseSeedingStartupAttribute>();
                if (seedingStartupAttribute != null) return seedingStartupAttribute.SeedingStartupType; // TODO: Check that it is not null, that is seeding startup type etc.

                seedingStartupAttribute = methodUnderTest.DeclaringType.GetCustomAttribute<UseSeedingStartupAttribute>();
                if (seedingStartupAttribute != null) return seedingStartupAttribute.SeedingStartupType; // TODO: Check that it is not null, that is seeding startup type etc.

                seedingStartupAttribute = methodUnderTest.DeclaringType.Assembly.GetCustomAttribute<UseSeedingStartupAttribute>();
                if (seedingStartupAttribute != null) return seedingStartupAttribute.SeedingStartupType; // TODO: Check that it is not null, that is seeding startup type etc.

                return null;
            }
        }
    }
}
