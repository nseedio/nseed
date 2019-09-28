using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using System;
using System.Reflection;

namespace NSeed.Cli.Subcommands.Info.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ProjectDefaultValueProvider : Attribute, IMemberConvention
    {
        public void Apply(ConventionContext context, MemberInfo member)
        {
            context.Application.OnParsingComplete(_ =>
            {
                var project = context.GetStringValue(nameof(InfoSubcommand.SeedProject));
                var fileSystemService = context.Application.GetService<IFileSystemService>();

                if (project.IsNotProvidedByUser())
                {
                }
                else
                {
                }
            });
        }
    }
}
