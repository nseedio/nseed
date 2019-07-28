using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace NSeed.Cli
{
    [Command(Name = "", Description = "Seed bucket command line tool.")]
    [Subcommand(typeof(Subcommands.Info.Subcommand))]
    [HelpOption]
    internal class Handle
    {
        public Task OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();

            return Task.CompletedTask;
        }
    }
}
