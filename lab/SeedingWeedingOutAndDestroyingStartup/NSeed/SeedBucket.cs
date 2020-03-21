using Microsoft.Extensions.DependencyInjection;
using NSeed.Abstractions;
using NSeed.Cli;
using NSeed.Discovery.SeedBucket;
using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.MetaInfo;
using NSeed.Seeding;
using System;
using System.Linq;
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
        /// TODO.
        /// </summary>
        /// <param name="output">TODO. TODO.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task Seed(IOutputSink output) // TODO: Think of dependency injection of the engine in generall and especially for IOutputSink.
        {
            return new Seeder(seedBucketInfoBuilder, output).SeedSeedBucket(GetType());
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
            // This method *must* be bullet-proof and must never throw any
            // exceptions. That's why the rigorous error checking of end user
            // errors that should never happen if the users follow the given
            // project templates and the NSeed documentation.

            string? errorMessage = null;

            if (commandLineArguments == null)
                errorMessage = GetErrorMessageForNullCommandLineArguments();

            if (errorMessage == null && commandLineArguments.Any(argument => argument == null))
                errorMessage = GetErrorMessageForCommandLineArgumentsWithNulls();

            TSeedBucket? seedBucket = null;
            if (errorMessage == null)
            {
                try
                {
                    seedBucket = new TSeedBucket();
                }
                catch (Exception exception)
                {
                    errorMessage = GetErrorMessageForExceptionInTheSeedBucketConstructor(exception);
                }
            }

            if (errorMessage != null)
            {
                ConsoleOutputSink.ShowInitializationErrorMessage(errorMessage);
                return 1;
            }

            // If we got this far, we know that both commandLineArguments and seedBucket exist; therefore a safe "!".
            return await CommandLineApplicationExecutor.Execute<MainCommand>(commandLineArguments!, services =>
            {
                services.AddSingleton<SeedBucket>(seedBucket!);
            });

            static string GetErrorMessageForNullCommandLineArguments()
            {
                return
                    $"Command line arguments passed to the {nameof(SeedBucket)}.{nameof(Handle)}() method were null." +
                    Environment.NewLine + Environment.NewLine +
                    $"Command line arguments passed to the {nameof(SeedBucket)}.{nameof(Handle)}() method must not be null." +
                    GetCommonExplanationMessage();
            }

            static string GetErrorMessageForCommandLineArgumentsWithNulls()
            {
                return
                    $"Command line arguments passed to the {nameof(SeedBucket)}.{nameof(Handle)}() method contain null elements." +
                    Environment.NewLine + Environment.NewLine +
                    $"Command line arguments passed to the {nameof(SeedBucket)}.{nameof(Handle)}() method must not contain null elements." +
                    GetCommonExplanationMessage();
            }

            static string GetErrorMessageForExceptionInTheSeedBucketConstructor(Exception exception)
            {
                return
                    $"An exception occured during the creation of the {typeof(TSeedBucket).FullName} object." +
                    Environment.NewLine +
                    "The exception was:" +
                    Environment.NewLine + Environment.NewLine +
                    exception.ToString() +
                    GetCommonExplanationMessage();
            }

            static string GetCommonExplanationMessage()
            {
                return
                    Environment.NewLine + Environment.NewLine +
                    "Check the implementation of your Main method." +
                    Environment.NewLine +
                    $"If you use the standard implementation of the {nameof(SeedBucket)} Main method, the above error will never happen." +
                    Environment.NewLine + Environment.NewLine +
                    "The standard implementation looks like this:" +
                    Environment.NewLine + Environment.NewLine +
                    $"class {typeof(TSeedBucket).Name} : SeedBucket" + Environment.NewLine +
                    "{" + Environment.NewLine +
                    "    static async Task<int> Main(string[] args)" + Environment.NewLine +
                    $"       => await Handle<{typeof(TSeedBucket).Name}>(args);" + Environment.NewLine +
                    "}";
            }
        }
    }
}
