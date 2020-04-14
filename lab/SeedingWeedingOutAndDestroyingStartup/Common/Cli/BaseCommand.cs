using McMaster.Extensions.CommandLineUtils;
using NSeed.Abstractions;
using NSeed.Cli.Assets;

namespace NSeed.Cli
{
    internal abstract class BaseCommand
    {
        public const string VerboseLongName = "verbose";
        public const string VerboseLongOption = "--" + VerboseLongName;
        public const string VerboseShortOption = "-v";

        [Option(VerboseShortOption + "|" + VerboseLongOption, Description = Resources.BaseCommand.VerboseDescription)]
        public bool Verbose { get; private set; }

        public const string NoColorLongName = "no-color";
        public const string NoColorLongOption = "--" + NoColorLongName;
        public const string NoColorShortOption = "-nc";

        [Option(NoColorShortOption + "|" + NoColorLongOption, Description = Resources.BaseCommand.NoColorDescription)]
        public bool NoColor { get; private set; }

        public const string FilterLongName = "filter";
        public const string FilterLongOption = "--" + FilterLongName;
        public const string FilterShortOption = "-f";

        [Option(FilterShortOption + "|" + FilterLongOption, Description = Resources.BaseCommand.FilterDescription)]
        public string Filter { get; private set; }

        protected IOutputSink Output { get; }

        protected BaseCommand(IOutputSink output)
        {
            Output = output;
            Filter = string.Empty;
        }
    }
}
