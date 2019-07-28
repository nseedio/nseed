using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli;
using NSeed.Discovery.SeedBucket;
using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Guards;
using NSeed.MetaInfo;
using System;
using System.IO;
using System.Reflection;
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
            return seedBucketInfoBuilder.BuildFrom(GetType());
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

            var serviceProvider = BuildServiceProvider(seedBucket);

            var app = new CommandLineApplication<Handle>();

            // TODO: Put this into some common CommandLineApplication executor
            //       so that the behaviour is the same between the Tool Cli and the Engine Cli.
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(serviceProvider);
            app.Name = GetSeedBucketExecutableName();
            app.MakeSuggestionsInErrorMessage = true;
            app.UsePagerForHelpText = false;

            try
            {
                return await app.ExecuteAsync(commandLineArguments);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return 1;
            }

            static string GetSeedBucketExecutableName()
            {
                const string fallbackExecutableName = "<your seed bucket>";

                // We will implement this method being highly paranoic. Highly.

                string? assemblyLocation = Assembly.GetEntryAssembly()?.Location;
                if (assemblyLocation == null) return fallbackExecutableName;

                string executableName = Path.GetFileNameWithoutExtension(assemblyLocation);
                return string.IsNullOrWhiteSpace(executableName)
                    ? fallbackExecutableName
                    : executableName;
            }

            static IServiceProvider BuildServiceProvider(SeedBucket seedBucket)
            {
                return new ServiceCollection()
                    .AddSingleton(PhysicalConsole.Singleton)
                    .AddSingleton(seedBucket)
                    .BuildServiceProvider();
            }
        }
    }
}
