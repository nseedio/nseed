using McMaster.Extensions.CommandLineUtils.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using System;
using System.Collections.Generic;
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

                IOperationResponse<IEnumerable<string>> response = project.IsNotProvidedByUser()
                 ? fileSystemService.GetNSeedProjectPaths(InitDirectory)
                 : fileSystemService.GetNSeedProjectPaths(project);

                if (context.ModelAccessor?.GetModel() is InfoSubcommand model && model != null)
                {
                    if (response.IsSuccessful)
                    {
                        var dependencyGraphService = context.Application.GetService<IDependencyGraphService>();

                        var projectPaths = response!.Payload;

                        foreach (var path in projectPaths!)
                        {
                            var frameworkResponse = dependencyGraphService.GetProjectFramework(path);

                            if (frameworkResponse.IsSuccessful && frameworkResponse.Payload!.IsDefined)
                            {
                                var resolvedProject = new Project(path, frameworkResponse.Payload);

                                var verifier = GetSeedBucketVerifier(frameworkResponse.Payload);

                                var verifyResponse = verifier.Verify(resolvedProject);

                                if (!verifyResponse.IsSuccessful)
                                {
                                    resolvedProject.ErrorMessage = verifyResponse.Message;
                                }

                                model.SetResolvedProject(resolvedProject);
                            }
                        }
                    }
                }

                ISeedBucketVerifier GetSeedBucketVerifier(IFramework framework)
                {
                    var seedBucketVerifierDiReolver = context.Application.GetService<Func<FrameworkType, ISeedBucketVerifier>>();
                    return seedBucketVerifierDiReolver(framework.Type);
                }
            });
        }
    }
}
