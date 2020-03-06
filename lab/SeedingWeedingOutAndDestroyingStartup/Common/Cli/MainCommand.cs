using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace NSeed.Cli
{
    [Command(Name = "nseed", Description = "Data seeding tool for .NET.")]
    [Subcommand(typeof(Subcommands.Info.InfoSubcommand))]
    [Subcommand(typeof(Subcommands.Seed.SeedSubcommand))]
    [HelpOption(Description = "Show command line help.", Inherited = true)]
    internal partial class MainCommand
    {
        public Task OnExecute(CommandLineApplication app)
        {
            app.ShowHelp(usePager: false);

            return Task.CompletedTask;
        }
    }
}
