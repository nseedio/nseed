using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Abstractions;
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
            var output = CreateConsoleOutputSink(false, false);

            var serviceCollection = CreateDefaultServiceCollection(output);

            serviceConfigurator?.Invoke(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var app = new CommandLineApplication<TApp>();

            var res = app.Parse(commandLineArguments);

            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(serviceProvider);
            app.Name = executableName ?? GetExecutableName();
            app.MakeSuggestionsInErrorMessage = true;
            app.UsePagerForHelpText = false;
            app.VerboseOption();

            try
            {
                return await app.ExecuteAsync(commandLineArguments);
            }
            catch (Exception exception)
            {
                output.WriteError(exception.Message);
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

            static IServiceCollection CreateDefaultServiceCollection(IOutputSink outputSink)
            {
                return new ServiceCollection()
                    .AddSingleton(PhysicalConsole.Singleton)
                    .AddSingleton(outputSink);
            }

            static IOutputSink CreateConsoleOutputSink(bool noColor, bool acceptsVerboseMessages)
            {
                var textColorsTheme = TextColorsTheme.GetForCurrentOS();
                var textColors = new TextColors(textColorsTheme, Console.ForegroundColor, Console.BackgroundColor, noColor);

                return new ConsoleOutputSink(textColors, acceptsVerboseMessages);
            }
        }
    }
}
