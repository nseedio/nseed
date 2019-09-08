using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using System;
using System.Reflection;
using System.Runtime.Versioning;

namespace NSeed.Cli.Subcommands.New.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class FrameworkDefaultValueProvider : Attribute, IMemberConvention
    {
        public void Apply(ConventionContext context, MemberInfo member)
        {
            context.Application.OnParsingComplete(_ =>
            {
                var framework = context.GetStringValue(nameof(NewSubcommand.Framework));
                var model = context.ModelAccessor?.GetModel() as NewSubcommand;
                model?.SetResolvedFramework(framework);
                if (framework.IsNotProvidedByUser())
                {
                    var dependencyGraphService = context.Application.GetService<IDependencyGraphService>();
                    model?.ResolveFramework(dependencyGraphService);
                }
            });
        }
    }
}
