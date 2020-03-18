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
    public sealed class RequiresSeedsAttribute : BeforeAfterTestAttribute
    {
        private readonly Type[] seedTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresSeedsAttribute"/> class.
        /// </summary>
        /// <param name="seedTypes">TODO.</param>
        public RequiresSeedsAttribute(params Type[] seedTypes)
        {
            // TODO: Check not null, must be seed.

            // TODO: During the execution do all the possible combinations: seedables from the same seed bucket, seed bucket only, several seed buckets, combinations...

            this.seedTypes = seedTypes;
        }

        /// <inheritdoc/>
        public override void Before(MethodInfo methodUnderTest)
        {
            base.Before(methodUnderTest);

            // TODO: Find a Startup within the test assembly.
        }
    }
}
