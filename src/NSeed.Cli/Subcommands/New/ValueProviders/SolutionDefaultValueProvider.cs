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
                (bool IsSuccesful, string Message) response = (false, string.Empty);
                if (solution.IsNotProvidedByUser())
                {
                    // Current working directory
                    response = fileSystemService.TryGetSolutionPath(InitDirectory, out solution);
                }
                else
                {
                    response = fileSystemService.TryGetSolutionPath(solution, out solution);
                }

                if (context.ModelAccessor?.GetModel() is NewSubcommand model && model != null)
                {
                    if (!response.IsSuccesful && response.Message.Exists())
                    {
                        model.SetResolvedSolutionErrorMessage(response.Message);
                    }

                    model?.SetResolvedSolution(solution);
                    var solutionValidator = context.GetValidator<SolutionValidator>();
                    solutionValidator.Validate(model!);
                }
            });
        }
    }
}
