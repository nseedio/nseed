using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Assets;

namespace NSeed.Cli
{
    internal abstract class BaseCommand
    {
        public const string VerboseLongName = "verbose";
        public const string VerboseLongOption = "--" + VerboseLongName;

        [Option("-v|" + VerboseLongOption, Description = Resources.BaseCommand.VerboseDescription)]
        public bool Verbose { get; private set; }

        public const string NoColorLongName = "no-color";

        [Option("-nc|--" + NoColorLongName, Description = Resources.BaseCommand.NoColorDescription)]
        public bool NoColor { get; private set; }
    }
}
