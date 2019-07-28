using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.Info
{
    [Command("info", Description = Resources.Info.CommandDescription)]
    internal class Subcommand
    {
        public Task OnExecute(CommandLineApplication app)
        {
            Console.WriteLine("\nExecute 'info' subcommand...\n");

            return Task.CompletedTask;
        }
    }
}
