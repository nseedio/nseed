using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Assets;

namespace NSeed.Cli.Subcommands.Info
{
    [Command("info", Description = Resources.Info.CommandDescription)]
    internal partial class InfoSubcommand : BaseCommand
    {
    }
}
