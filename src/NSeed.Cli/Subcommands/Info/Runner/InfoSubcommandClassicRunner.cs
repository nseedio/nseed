using NSeed.Cli.Runners;
using System;
using System.IO;

namespace NSeed.Cli.Subcommands.Info.Runner
{
    internal class InfoSubcommandClassicRunner : DotNetRunner, IDotNetRunner<InfoSubcommandRunnerArgs>
    {
        public (bool IsSuccessful, string Message) Run(InfoSubcommandRunnerArgs args)
        {
            var tempBuildOutput = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var arguments = new[] { "build", args.NSeedProjectPath, $"-o {tempBuildOutput}" };
            var exeCommand = Path.Combine(tempBuildOutput, $"{args.NSeedProjectName}.exe");
            var response = Response(Run(exeCommand, args.NSeedProjectDirectory, arguments));
            return response;
        }
    }
}
