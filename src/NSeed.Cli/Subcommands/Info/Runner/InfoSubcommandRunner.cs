using NSeed.Cli.Runners;

namespace NSeed.Cli.Subcommands.Info.Runner
{
    internal class InfoSubcommandRunner : DotNetRunner, IDotNetRunner<InfoSubcommandRunnerArgs>
    {
        public (bool IsSuccessful, string Message) Run(InfoSubcommandRunnerArgs args)
        {
            var arguments = new[] { "build", args.NseedProject };
            var response = Response(Run(args.NseedProjectDirectory, arguments));

            if (response.IsSuccessful)
            {
                arguments = new[] { "publish", args.NseedProject };
                response = Response(Run(args.NseedProjectDirectory, arguments));
            }

            return response;
        }
    }
}
