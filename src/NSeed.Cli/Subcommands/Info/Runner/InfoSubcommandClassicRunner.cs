using NSeed.Cli.Runners;
using System;
using System.IO;

namespace NSeed.Cli.Subcommands.Info.Runner
{
    internal class InfoSubcommandClassicRunner : DotNetRunner, IDotNetRunner<InfoSubcommandRunnerArgs>
    {
        public (bool IsSuccessful, string Message) Run(InfoSubcommandRunnerArgs args)
        {
            var tempBuildOutput = Path.Combine(Path.GetTempPath(), $"BucketBuild{Guid.NewGuid().ToString()}");

            var arguments = new[] { "build", args.NSeedProjectPath, $"-o {tempBuildOutput}" };
            var response = RunDotNet(args.NSeedProjectDirectory, arguments);
            if (!response.IsSuccess)
            {
                var error = response.Errors;
                if (string.IsNullOrEmpty(error))
                {
                    // TODO: Log response. Output.
                    error = Assets.Resources.Info.Errors.SeedBucketProjectCouldNotBeBuilt;
                }

                Directory.Delete(tempBuildOutput, true);
                return (false, error);
            }

            var exeCommand = Path.Combine(tempBuildOutput, $"{args.NSeedProjectName}.exe");
            RunSeedBucket(exeCommand, args.NSeedProjectDirectory, new[] { "info" });

            Directory.Delete(tempBuildOutput, true);
            return (true, string.Empty);
        }
    }
}
