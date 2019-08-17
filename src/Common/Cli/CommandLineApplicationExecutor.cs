using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace NSeed.Cli
{
    internal static class CommandLineApplicationExecutor
    {
        public static async Task<int> Execute<TApp>(string[] commandLineArguments, Action<IServiceCollection>? serviceConfigurator = null, string? executableName = null)
            where TApp : class
        {
            var serviceCollection = BuildDefaultServiceCollection();

            serviceConfigurator?.Invoke(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var app = new CommandLineApplication<TApp>();

            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(serviceProvider);
            app.Name = executableName ?? GetExecutableName();
            app.MakeSuggestionsInErrorMessage = true;
            app.UsePagerForHelpText = false;

            try
            {
                return await app.ExecuteAsync(commandLineArguments);
            }
            catch (Exception exception)
            {
                // TODO: Add output sink here.
                Console.WriteLine(exception.Message);
                return 1;
            }

            static string GetExecutableName()
            {
                const string fallbackExecutableName = "NSeed";

                // We will implement this method being highly paranoic. Highly.

                string? assemblyLocation = Assembly.GetEntryAssembly()?.Location;
                if (assemblyLocation == null) return fallbackExecutableName;

                string executableName = Path.GetFileNameWithoutExtension(assemblyLocation);
                return string.IsNullOrWhiteSpace(executableName)
                    ? fallbackExecutableName
                    : executableName;
            }

            static IServiceCollection BuildDefaultServiceCollection()
            {
                return new ServiceCollection()
                    .AddSingleton(PhysicalConsole.Singleton);
            }
        }
    }
}
