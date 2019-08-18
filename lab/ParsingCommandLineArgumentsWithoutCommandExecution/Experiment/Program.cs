using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using System;
using System.Linq;

namespace Experiment
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var app = new CommandLineApplication<MainCommand>();
            app.Conventions.UseDefaultConventions();
            app.ThrowOnUnexpectedArgument = false;

            Console.WriteLine($"Args:");

            foreach (var argument in args)
            {
                Console.WriteLine(argument);
            }
            
            Console.WriteLine();

            try
            {
                var parseResult = app.Parse(args);

                Console.WriteLine($"Selected command: {parseResult.SelectedCommand.Name}");

                Console.WriteLine();

                var verbose = IsBooleanOptionSet(parseResult, "verbose");
                var noColor = IsBooleanOptionSet(parseResult, "no-color");

                Console.WriteLine($"Verbose: {verbose}");
                Console.WriteLine($"NoColor: {noColor}");
            }
            catch (Exception)
            {
                Console.WriteLine("Parsing error.");
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

    internal abstract class BaseCommand
    {
        [Option("-v|--verbose", Description = "Show verbose output")]
        public bool Verbose { get; private set; }

        [Option("-nc|--no-color", Description = "Do not use colored output")]
        public bool NoColor { get; private set; }
    }

    [Subcommand(typeof(BaseCommand))]
    [Command("new", Description = "Create something new")]
    internal class New : BaseCommand
    {
    }

    [Subcommand(typeof(BaseCommand))]
    [Command("info", Description = "Get some info")]
    internal class Info : BaseCommand
    {
    }

    [Subcommand(typeof(New), typeof(Info))]
    [Command(Name = "experiment", Description = "Command line parsing experiment.")]
    internal class MainCommand
    {
    }
}
