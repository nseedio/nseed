using McMaster.Extensions.CommandLineUtils.Abstractions;
using System.Linq;

namespace NSeed.Cli.Extensions
{
    internal static class ParseResultExtension
    {
        public static bool ShowHelp(this ParseResult context)
        {
            var showHelp = context?.SelectedCommand?.OptionHelp?.Values?.Any();
            return showHelp.HasValue && showHelp.Value;
        }
    }
}
