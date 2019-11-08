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
                return (false, response.Errors);
            }

            var exeCommand = Path.Combine(tempBuildOutput, $"{args.NSeedProjectName}.exe");
            RunSeedBucket(exeCommand, args.NSeedProjectDirectory, new[] { "info" });
            return (true, string.Empty);
        }
    }
}