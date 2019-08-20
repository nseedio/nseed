using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NSeed.Cli
{
    internal static class CommandLineApplicationExecutor
    {
        public static async Task<int> Execute<TApp>(string[] commandLineArguments, Action<IServiceCollection>? serviceConfigurator = null, string? executableName = null)
            where TApp : class
        {
            // For no-color see: https://no-color.org
            var (noColor, verbose) = GetNoColorAndVerboseCommandLineOptions(commandLineArguments);
            if (!noColor) noColor = Environment.GetEnvironmentVariable("NO_COLOR") != null;

            IOutputSink output = ConsoleOutputSink.Create(noColor, verbose);

            var serviceCollection = CreateDefaultServiceCollection(output);

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

            static (bool noColor, bool verbose) GetNoColorAndVerboseCommandLineOptions(string[] commandLineArguments)
            {
                var app = new CommandLineApplication<TApp>();
                app.Conventions.UseDefaultConventions();
                app.ThrowOnUnexpectedArgument = false;

                try
                {
                    var parseResult = app.Parse(commandLineArguments);

                    var noColor = IsBooleanOptionSet(parseResult, BaseCommand.NoColorLongName);
                    var verbose = IsBooleanOptionSet(parseResult, BaseCommand.VerboseLongName);

                    return (noColor, verbose);
                }
                catch (Exception)
                {
                    return (false, false);
                }

                static bool IsBooleanOptionSet(ParseResult parseResult, string optionLongName)
                {
                    return parseResult
                        .SelectedCommand
                        .GetOptions()
                        .Any(option => option.LongName == optionLongName && option.Values.Count > 0);
                }
            }
        }
    }
}
