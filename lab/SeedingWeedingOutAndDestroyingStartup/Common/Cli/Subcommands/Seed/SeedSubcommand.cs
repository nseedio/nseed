using McMaster.Extensions.CommandLineUtils;
using NSeed.Abstractions;
using NSeed.Cli.Assets;

namespace NSeed.Cli.Subcommands.Seed
{
    [Command("seed", Description = Resources.Seed.CommandDescription)]
    internal partial class SeedSubcommand : BaseCommand
    {
    }
}
