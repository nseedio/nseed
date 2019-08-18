using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using System;
using System.Reflection;

namespace NSeed.Cli.Subcommands.New.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class NameDefaultValueProvider : Attribute, IMemberConvention
    {
        private readonly string defaultName;

        public NameDefaultValueProvider(string defaultName)
        {
            this.defaultName = defaultName;
        }

        public void Apply(ConventionContext context, MemberInfo member)
        {
            context.Application.OnParsingComplete(_ =>
            {
                var name = context.GetStringValue(nameof(NewSubcommand.Name));
                var model = context.ModelAccessor?.GetModel() as NewSubcommand;
                model?.SetResolvedName(name);
                if (name.IsNotProvidedByUser())
                {
                    var dependencyGraphService = context.Application.GetService<IDependencyGraphService>();
                    model?.ResolveDefaultNameWithPrefix(dependencyGraphService, defaultName);
                }
            });
        }
    }
}
