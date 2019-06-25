using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using System;
using System.Linq;
using System.Reflection;

namespace NSeed.Cli.Subcommands.New.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FrameworkDefaultValueProvider : Attribute, IMemberConvention
    {
        public void Apply(ConventionContext context, MemberInfo member)
        {
            context.Application.OnParsingComplete(_ =>
            {
                var framework = context.GetValue<string>(nameof(Subcommand.Framework));
                if (framework.IsNotProvidedByUser())
                {
                    var solutionValidator = context.GetValidator<SolutionValidator>();
                    if (solutionValidator.Validate().IsValid)
                    {
                        var solution = context.GetValue<string>(nameof(Subcommand.Solution));
                        var dependencyGraphService = context.Application.GetService<IDependencyGraphService>();
                        var commonFramework = dependencyGraphService.GetProjectsFramework(solution);
                        if (commonFramework.Exists())
                        {
                            context.SetValue(nameof(Subcommand.Framework), framework);
                        }
                    }
                }
            });
        }
    }
}
