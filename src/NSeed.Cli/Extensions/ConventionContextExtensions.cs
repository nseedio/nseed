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
        public static T GetValue<T>(this ConventionContext context, string parameterName)
            where T : class
        {
            var optionValue = GetCommandOptionsByLongName(context.Application.GetOptions(), parameterName)
                .FirstOrDefault();
            return optionValue as T;
        }

        public static void SetValue(this ConventionContext context, string parameterName, string value)
        {
            var solutionValues = GetCommandOptionsByLongName(context.Application.GetOptions(), parameterName).ToList();
            solutionValues.Clear();
        }

        public static IValidator<NewSubcommand> GetValidator<T>(this ConventionContext context)
            where T : IValidator<NewSubcommand>
        {
            return context.Application
                    .GetServices<IValidator<NewSubcommand>>()
                    .FirstOrDefault(s => s.GetType() == typeof(T));
        }

        private static IEnumerable<string> GetCommandOptionsByLongName(IEnumerable<CommandOption> options, string longName)
        {
            options.AreEmptyIfNull();
            return options.FirstOrDefault(b => b.LongName.Equals(
                longName,
                StringComparison.InvariantCultureIgnoreCase))
                ?.Values ?? Enumerable.Empty<string>();
        }
    }
}
