using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Extensions;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Services;
using NSeed.Cli.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.New.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NameDefaultValueProvider : Attribute, IMemberConvention
    {
        private readonly string DefaultName;

        public NameDefaultValueProvider(string defaultName)
        {
            DefaultName = defaultName;
        }

        public void Apply(ConventionContext context, MemberInfo member)
        {
            //Adds an action to be invoked when all command line arguments have been parsed and validated.
            context.Application.OnParsingComplete(_ =>
            {
                var name = context.GetValue<string>(nameof(Subcommand.Name));
                if (name.IsNotProvidedByUser())
                {
                    var solutionValidator = context.GetValidator<SolutionValidator>();
                    if (solutionValidator.Validate().IsValid)
                    {
                        var solution = context.GetValue<string>(nameof(Subcommand.Solution));
                        var dependencyGraphService = context.Application.GetService<IDependencyGraphService>();
                        var commonPrefix = dependencyGraphService.GetProjectsPrefix(solution);
                        if (commonPrefix.Exists())
                        {
                            context.SetValue(nameof(Subcommand.Name), $"{commonPrefix}.{DefaultName}");
                        }
                    }
                }
            });
        }
    }
}
