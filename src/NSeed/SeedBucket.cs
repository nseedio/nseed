using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli;
using NSeed.Discovery.SeedBucket;
using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Guards;
using NSeed.MetaInfo;
using System;
using System.Threading.Tasks;

namespace NSeed
{
    /// <summary>
    /// A bucket of <see cref="ISeed"/>s.
    /// </summary>
    public abstract class SeedBucket
    {
        private readonly ISeedBucketInfoBuilder<Type> seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

        /// <summary>
        /// Gets <see cref="SeedBucketInfo"/> for this seed bucket.
        /// </summary>
        /// <returns><see cref="SeedBucketInfo"/> that describes this seed bucket.</returns>
        public SeedBucketInfo GetMetaInfo()
        {
            // We know that the seed bucket info builder always returns
            // a seed bucket info and never null; threfore "!".
            return seedBucketInfoBuilder.BuildFrom(GetType())!;
        }

        /// <summary>
        /// Handles the commands provided by <paramref name="commandLineArguments"/> and
        /// writes output to the console.
        /// </summary>
        /// <typeparam name="TSeedBucket">
        /// The type of the <see cref="SeedBucket"/> that has to be handled.
        /// </typeparam>
        /// <param name="commandLineArguments">The command line arguments to handle.</param>
        /// <returns>The <see cref="Task"/> representing the asynchronous handling operation.</returns>
        protected static async Task<int> Handle<TSeedBucket>(string[] commandLineArguments)
            where TSeedBucket : SeedBucket, new()
        {
            commandLineArguments.MustNotBeNull(nameof(commandLineArguments));
            commandLineArguments.MustNotContain((string?)null, nameof(commandLineArguments));

            // TODO: Add error checking. Should this be in try-catch?
            var seedBucket = new TSeedBucket();

            return await CommandLineApplicationExecutor.Execute<MainCommand>(commandLineArguments, services =>
            {
                services.AddSingleton(seedBucket);
            });
        }
    }
}
