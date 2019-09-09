using NSeed.Cli.Subcommands.New.Models;

namespace NSeed.Cli.Subcommands.New.Runner
{
    internal class NewSubcommandRunnerArgs
    {
        public Template Template { get; set; } = new Template();

        public string SolutionDirectory { get; set; } = string.Empty;

        public string Solution { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Framework { get; set; } = string.Empty;
    }
}
