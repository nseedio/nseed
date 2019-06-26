using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using System;
using System.Reflection;
using static NSeed.Cli.Resources.Resources;

namespace NSeed.Cli.Subcommands.New.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class SolutionDefaultValueProvider : Attribute, IMemberConvention
    {
        public void Apply(ConventionContext context, MemberInfo member)
        {
            //Adds an action to be invoked when all command line arguments have been parsed and validated.
            context.Application.OnParsingComplete(_ =>
            {
                var solution = context.GetValue<string>(nameof(Subcommand.Solution));
                if (solution.IsNotProvidedByUser())
                {
                    var fileSystemService = context.Application.GetService<IFileSystemService>();
                    solution = fileSystemService.GetSolution(InitDirectory);
                    if (solution.Exists())
                    {
                        context.SetValue(nameof(Subcommand.Solution), solution);
                    }
                }
            });
        }
    }
}
