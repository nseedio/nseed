using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Abstractions;
using System;
using System.Collections.Generic;
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
            // This method is actually the application top level entry point.
            // It *must* be bullet-proof and must never throw any exceptions.
            // That's why the whole code is in the try-catch block.

            IOutputSink? output = null;
            try
            {
                // For no-color see: https://no-color.org
                var (noColor, verbose) = GetNoColorAndVerboseCommandLineOptions(commandLineArguments);
                if (!noColor) noColor = Environment.GetEnvironmentVariable("NO_COLOR") != null;

                output = ConsoleOutputSink.Create(noColor, verbose);

                var serviceCollection = CreateDefaultServiceCollection(output);

                serviceConfigurator?.Invoke(serviceCollection);

                var serviceProvider = serviceCollection.BuildServiceProvider();

                var app = new CommandLineApplication<TApp>();

                app.Conventions
                    .UseDefaultConventions()
                    .UseConstructorInjection(serviceProvider);
                app.Name = executableName ?? GetExecutableName();
                app.ThrowOnUnexpectedArgument = false;
                app.MakeSuggestionsInErrorMessage = true;
                app.UsePagerForHelpText = false;
                app.ShowHint();

                return await app.ExecuteAsync(commandLineArguments);
            }
            catch (Exception exception)
            {
                var message = GetEmbarrassingInternalErrorMessage(exception, commandLineArguments);

                if (output != null)
                    output.WriteError(message);
                else
                    ConsoleOutputSink.ShowInitializationErrorMessage(message);

                return 1;
            }

            static string GetEmbarrassingInternalErrorMessage(Exception exception, string[] commandLineArguments)
            {
                // The Ghostbusters ASCII has been created by using:
                //    https://helloacm.com/cowsay/?msg=Uups%21+It+looks+like+we+have+a+gost+in+the+system+%3B-%29&f=ghostbusters&w=cowsay
                return
                    @" _____________________________________" + Environment.NewLine +
                    @"/ Uups! Looks like we have a ghost in \" + Environment.NewLine +
                    @"\ the system ;-)                      /" + Environment.NewLine +
                    @" -------------------------------------" + Environment.NewLine +
                    @"          \" + Environment.NewLine +
                    @"           \" + Environment.NewLine +
                    @"            \          __---__" + Environment.NewLine +
                    @"                    _-       /--______" + Environment.NewLine +
                    @"               __--( /     \ )XXXXXXXXXXX\v." + Environment.NewLine +
                    @"             .-XXX(   O   O  )XXXXXXXXXXXXXXX-" + Environment.NewLine +
                    @"            /XXX(       U     )        XXXXXXX\" + Environment.NewLine +
                    @"          /XXXXX(              )--_  XXXXXXXXXXX\" + Environment.NewLine +
                    @"         /XXXXX/ (      O     )   XXXXXX   \XXXXX\" + Environment.NewLine +
                    @"         XXXXX/   /            XXXXXX   \__ \XXXXX" + Environment.NewLine +
                    @"         XXXXXX__/          XXXXXX         \__---->" + Environment.NewLine +
                    @" ---___  XXX__/          XXXXXX      \__         /" + Environment.NewLine +
                    @"   \-  --__/   ___/\  XXXXXX            /  ___--/=" + Environment.NewLine +
                    @"    \-\    ___/    XXXXXX              '--- XXXXXX" + Environment.NewLine +
                    @"       \-\/XXX\ XXXXXX                      /XXXXX" + Environment.NewLine +
                    @"         \XXXXXXXXX   \                    /XXXXX/" + Environment.NewLine +
                    @"          \XXXXXX      >                 _/XXXXX/" + Environment.NewLine +
                    @"            \XXXXX--__/              __-- XXXX/" + Environment.NewLine +
                    @"             -XXXXXXXX---------------  XXXXXX-" + Environment.NewLine +
                    @"                \XXXXXXXXXXXXXXXXXXXXXXXXXX/" + Environment.NewLine +
                    @"                  ""VXXXXXXXXXXXXXXXXXXV""" +
                    Environment.NewLine + Environment.NewLine +
                    "Well, this is embarrassing :-(" +
                    Environment.NewLine +
                    "We have a ghost in the system." +
                    Environment.NewLine +
                    "It materialized itself as an exception:" +
                    Environment.NewLine + Environment.NewLine +
                    exception.ToString() +
                    Environment.NewLine + Environment.NewLine +
                    "Try running the same console command again." +
                    Environment.NewLine +
                    "If the ghost is still there, help us to bust it out:" +
                    Environment.NewLine +
                    " - Use the verbose option and redirect the output to Ghostbusting.txt:" +
                    Environment.NewLine +
                    "   -> " + GetVerboseCommandLine(commandLineArguments) + " > Ghostbusting.txt" +
                    Environment.NewLine +
                    " - Open an issue on GitHub an attach the Ghostbusting.txt to it:" +
                    Environment.NewLine +
                    "   -> https://github.com/nseedio/nseed/issues" +
                    Environment.NewLine +
                    " - Try to provide a Short, Self Contained, Correct/Compilable, Example:" +
                    Environment.NewLine +
                    "   -> http://sscce.org/" +
                    Environment.NewLine + Environment.NewLine +
                    "Thanks for helping us keeping NSeed ghost-free!";

                static string GetVerboseCommandLine(string[] commandLineArguments)
                {
                    string[] verboseOption = new[] { BaseCommand.VerboseLongOption };

                    IEnumerable<string> arguments;

                    // If there are additional arguments specified in the original command line (--)
                    // we have to put the --verbose option before them (before the --).
                    // Also, we have to check if the --verbose option is already set, but only before
                    // the additional arguments (if they exist).
                    int indexOfAdditionalArguments = Array.FindIndex(commandLineArguments, argument => argument == "--");
                    if (indexOfAdditionalArguments >= 0)
                    {
                        // We have additional arguments (--).
                        arguments = commandLineArguments.Take(indexOfAdditionalArguments).Contains(BaseCommand.VerboseLongOption)
                            ? commandLineArguments
                            : commandLineArguments.Take(indexOfAdditionalArguments)
                                .Concat(verboseOption)
                                .Concat(commandLineArguments.Skip(indexOfAdditionalArguments));
                    }
                    else
                    {
                        // We do not have additional arguments (--).
                        arguments = commandLineArguments.Contains(BaseCommand.VerboseLongOption)
                            ? commandLineArguments
                            : commandLineArguments.Concat(verboseOption);
                    }

                    return ArgumentEscaper.EscapeAndConcatenate(arguments);
                }
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
