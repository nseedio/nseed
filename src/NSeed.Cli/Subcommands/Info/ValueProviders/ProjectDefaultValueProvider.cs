using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Abstractions;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using System;
using System.Reflection;
using static NSeed.Cli.Assets.Resources;

namespace NSeed.Cli.Subcommands.Info.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ProjectDefaultValueProvider : Attribute, IMemberConvention
    {
        public void Apply(ConventionContext context, MemberInfo member)
        {
            context.Application.OnParsingComplete(_ =>
            {
                var project = context.GetStringValue(nameof(InfoSubcommand.Project));

                var fileSystemService = context.Application.GetService<IFileSystemService>();

                IOperationResponse<string> response = project.IsNotProvidedByUser()
                 ? fileSystemService.GetNSeedProjectPath(InitDirectory)
                 : fileSystemService.GetNSeedProjectPath(project);

                if (context.ModelAccessor?.GetModel() is InfoSubcommand model && model != null)
                {
                    if (!response.IsSuccessful && response.Message.Exists())
                    {
                        model.SetResolvedProjectErrorMessage(response.Message);
                    }

                    model?.SetResolvedProject(response?.Payload ?? string.Empty);
                }
            });
        }
    }
}
