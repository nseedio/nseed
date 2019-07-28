using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using System;
using System.Reflection;

namespace NSeed.Cli.Subcommands.New.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class FrameworkDefaultValueProvider : Attribute, IMemberConvention
    {
        public void Apply(ConventionContext context, MemberInfo member)
        {
            context.Application.OnParsingComplete(_ =>
            {
                var framework = context.GetValue<string>(nameof(Subcommand.Framework));
                var model = context.ModelAccessor.GetModel() as New.Subcommand;
                model.SetResolvedFramework(framework);
                if (framework.IsNotProvidedByUser())
                {
                    var dependencyGraphService = context.Application.GetService<IDependencyGraphService>();
                    model.ResolveFramework(dependencyGraphService);
                }
            });
        }
    }
}
