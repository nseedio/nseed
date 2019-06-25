using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Validation;
using System;
using System.Linq;

namespace NSeed.Cli.Extensions
{
    public static class ConventionContextExtensions
    {
        public static T GetValue<T>(this ConventionContext context, string parameterName) where T : class
        {
            var newSubcommandOptions = context.Application.GetOptions();
            var solution = newSubcommandOptions
                .FirstOrDefault(b => b.LongName.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase))
                .Values
                .FirstOrDefault();
            return solution as T;
        }

        public static void SetValue(this ConventionContext context, string parameterName, string value)
        {
            var newSubcommandOptions = context.Application.GetOptions();
            var solutionValues = newSubcommandOptions
                .FirstOrDefault(b => b.LongName.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase))
                .Values;
            if (solutionValues != null && solutionValues.Count == 1)
            {
                solutionValues[0] = value;
            }
            if (!solutionValues.Any())
            {
                solutionValues.Add(value);
            }
        }

        public static IValidator GetValidator<T>(this ConventionContext context) 
            where T : IValidator
        {
            return context.Application
                    .GetServices<IValidator>()
                    .FirstOrDefault(s => s.GetType() == typeof(T));
        }
    }
}