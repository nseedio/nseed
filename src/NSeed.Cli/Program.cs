using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NSeed.Cli
{
    [Command(Name = "nseed", Description = "Data seeding command line tool.")]
    [Subcommand(typeof(Subcommands.New.Subcommand))]
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await new HostBuilder()
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    services
                       .AddSingleton(PhysicalConsole.Singleton)
                       .AddSingleton<CommandLineApplication>()
                       .AddSingleton<IReporter>(provider => new ConsoleReporter(provider.GetService<IConsole>()));

                    Services.DiConfig.RegisterServices(services);
                    Subcommands.DiConfig.RegisterValidators(services);
                })
                .RunCommandLineApplicationAsync<Program>(args);
        }
    }
}
