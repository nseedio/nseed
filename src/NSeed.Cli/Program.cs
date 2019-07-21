using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSeed.Cli.Services;
using System;
using System.Threading.Tasks;

namespace NSeed.Cli
{
    [Command(Name = "nseed", Description = "Data seeding command line tool.")]
    [Subcommand(typeof(Subcommands.New.Subcommand))]
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
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

                           DiConfig.RegisterServices(services);
                           Subcommands.DiConfig.RegisterValidators(services);
                       })
                       .RunCommandLineApplicationAsync<Program>(args);
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
