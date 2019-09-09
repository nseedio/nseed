using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Cli.Extensions
{
    internal static class ConventionContextExtensions
    {
        public static string GetStringValue(this ConventionContext context, string parameterName)
        {
            var optionValue = GetCommandOptionsByLongName(
                context.Application.GetOptions() ?? new List<CommandOption>(),
                parameterName)?.FirstOrDefault();
            return optionValue ?? string.Empty;
        }

        public static IValidator<NewSubcommand> GetValidator<T>(this ConventionContext context)
            where T : IValidator<NewSubcommand>
        {
            return context.Application
                    .GetServices<IValidator<NewSubcommand>>()
                    .FirstOrDefault(s => s.GetType() == typeof(T));
        }

        private static IEnumerable<string?> GetCommandOptionsByLongName(
            IEnumerable<CommandOption> options,
            string longName)
        {
            return options
                .FirstOrDefault(b => b.LongName != null
                && b.LongName.Equals(longName, StringComparison.InvariantCultureIgnoreCase))?.Values
                ?? Enumerable.Empty<string?>();
        }
    }
}
