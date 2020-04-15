namespace NSeed.Cli.Assets
{
    internal static partial class Resources
    {
        internal static class BaseCommand
        {
            public const string VerboseDescription = "Show verbose output.";
            public const string NoColorDescription = "Do not use colored output.";
            public const string FilterDescription = "Seed or scenario filter. The filter is a part of the seed or scenario full type name."; // TODO: Extend with additional info once GLOB patterns and Regex is supported. Add support for several strings. Move to some other place because e.g. destroy command will not have filters. E.g. FilterableCommand : BaseCommand.
        }
    }
}
