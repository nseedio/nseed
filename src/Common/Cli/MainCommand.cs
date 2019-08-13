using McMaster.Extensions.CommandLineUtils;

namespace NSeed.Cli
{
    [Command(Name = "nseed", Description = "Data seeding command line tool.")]
    [Subcommand(typeof(Subcommands.Info.InfoSubcommand))]
    [HelpOption]
    internal partial class MainCommand
    {
    }
}
