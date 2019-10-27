using NSeed.Cli.Runners;
using System;

namespace NSeed.Cli.Subcommands.Info.Runner
{
    internal class InfoSubcommandCoreRunner : DotNetRunner, IDotNetRunner<InfoSubcommandRunnerArgs>
    {
        public (bool IsSuccessful, string Message) Run(InfoSubcommandRunnerArgs args)
        {
            var arguments = new[] { "run", "--project", args.NSeedProjectPath, "info" };
            RunDotNetSeedBucket(args.NSeedProjectDirectory, arguments);
            return (true, string.Empty);
        }
    }
}
