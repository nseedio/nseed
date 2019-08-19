using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Assets;

namespace NSeed.Cli
{
    internal abstract class BaseCommand
    {
        [Option("-v|--verbose", Description = Resources.BaseCommand.VerboseDescription)]
        public bool Verbose { get; private set; }

        [Option("-nc|--no-color", Description = Resources.BaseCommand.NoColorDescription)]
        public bool NoColor { get; private set; }
    }
}
