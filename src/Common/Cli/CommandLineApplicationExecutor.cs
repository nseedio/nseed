using McMaster.Extensions.CommandLineUtils;
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

                var consoleOutputSink = ConsoleOutputSink.Create(noColor, verbose);

                output = consoleOutputSink;

                var serviceCollection = CreateDefaultServiceCollection(consoleOutputSink);

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

                return await app.ExecuteAsync(commandLineArguments);
            }
            catch (UnrecognizedCommandParsingException exception)
            {
                // In this case, we know that the output sink object exists,
                // because the parsing happens after the output sink object
                // is created and assigned to the "output" variable.
                // So we can safely access it here; therefoe "!".
                var message = exception.Message;
                if (!message.EndsWith(".")) message += ".";

                output!.WriteMessage(message);

                if (exception.NearestMatches.Any())
                {
                    output!.WriteLine();
                    output!.WriteMessage($"Did you maybe mean '{exception.NearestMatches.First()}'?");
                }

                return 1;
            }
            catch (Exception exception)
            {
                var message = GetEmbarrassingInternalErrorMessage(exception, commandLineArguments);

                if (output != null)
                    output.WriteError(message);
                else
                    ConsoleOutputSink.ShowInitializationErrorMessage(message);

                return 2;
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

            static IServiceCollection CreateDefaultServiceCollection(ConsoleOutputSink outputSink)
            {
                return new ServiceCollection()
                    .AddSingleton(PhysicalConsole.Singleton)
                    .AddSingleton<IOutputSink>(outputSink)
                    .AddSingleton<ITextColorsProvider>(outputSink);
            }

            static (bool noColor, bool verbose) GetNoColorAndVerboseCommandLineOptions(string[] commandLineArguments)
            {
                // The reason why we need these two options parsed from the command
                // line before the "standard" parsing starts is explained in the
                // "Parsing Command Line Arguments Without Command Execution" lab:
                // See: /lab/ParsingCommandLineArgumentsWithoutCommandExecution/README.md

                // Originaly this method used CommandLineApplication for parsing, as explained
                // in the lab. But this was causing issues with help rendering.
                // See: https://github.com/nseedio/nseed/issues/1
                // That's why we switched to simple manual parsing.

                // Command line options are case sensitive.
                return
                (
                    commandLineArguments.Any(argument => argument == BaseCommand.NoColorShortOption || argument == BaseCommand.NoColorLongOption),
                    commandLineArguments.Any(argument => argument == BaseCommand.VerboseShortOption || argument == BaseCommand.VerboseLongOption)
                );
            }
        }
    }
}
