using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace NSeed.Cli
{
    internal class Handle
    {
        public Task OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();

            return Task.CompletedTask;
        }
    }
}
