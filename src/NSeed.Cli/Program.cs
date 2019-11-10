using NSeed.Cli.Runners;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands;
using System.Threading.Tasks;

namespace NSeed.Cli
{
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await CommandLineApplicationExecutor.Execute<MainCommand>(
                args,
                services =>
                {
                    services
                        .AddCliServices()
                        .AddRunners()
                        .AddSubcommandRunners()
                        .AddValidators()
                        .AddSeedBucketVerifier();
                },
                "NSeed");
        }
    }
}
