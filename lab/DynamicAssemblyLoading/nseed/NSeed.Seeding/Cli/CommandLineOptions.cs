using System.Collections.Generic;
using CommandLine;

namespace NSeed.Cli
{
    internal class CommandLineOptions
    {
        public class CommandVerbs
        {
            public const string List = "list";
            public const string Seed = "seed";
        }
        
        public ListVerbCommandOptions ListVerb { get; set; }
        
        public SeedVerbCommandOptions SeedVerb { get; set; }
    }

    internal abstract class SubOptionsBase
    {
        [Option('f', "filters", Required = false, HelpText = @"One or more filters for the seeds to seed, separated by space. E.g. MyProject.Administrators MyProject.Customers products invoices")]
        public IList<string> Seeds { get; set; }        
    }

    [Verb(CommandLineOptions.CommandVerbs.List, HelpText = "Lists the seeds contained in the seed bucket.")]
    internal class ListVerbCommandOptions : SubOptionsBase { }

    [Verb(CommandLineOptions.CommandVerbs.Seed, HelpText = "Seeds the seeds contained in the seed bucket.")]
    internal class SeedVerbCommandOptions : SubOptionsBase { }
}