using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New.Validators;
using System;
using System.Reflection;
using static NSeed.Cli.Assets.Resources;

namespace NSeed.Cli.Subcommands.New.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class SolutionDefaultValueProvider : Attribute, IMemberConvention
    {
        public void Apply(ConventionContext context, MemberInfo member)
        {
            context.Application.OnParsingComplete(_ =>
            {
                var solution = context.GetStringValue(nameof(NewSubcommand.Solution));
                var fileSystemService = context.Application.GetService<IFileSystemService>();
                if (solution.IsNotProvidedByUser())
                {
                    fileSystemService.TryGetSolutionPath(InitDirectory, out solution);
                }
                else
                {
                    fileSystemService.TryGetSolutionPath(solution, out solution);
                }

                var model = context.ModelAccessor?.GetModel() as NewSubcommand;
                model?.SetResolvedSolution(solution);
                var solutionValidator = context.GetValidator<SolutionValidator>();
                if (model != null)
                {
                    solutionValidator.Validate(model);
                }
            });
        }
    }
}
