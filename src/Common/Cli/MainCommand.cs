using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace NSeed.Cli
{
    [Command(Name = "nseed", Description = "Data seeding command line tool.")]
    [Subcommand(typeof(Subcommands.Info.InfoSubcommand))]
    [HelpOption]
    internal partial class MainCommand
    {
        public Task OnExecute(CommandLineApplication app)
        {
            return Task.CompletedTask;
        }
    }
}
