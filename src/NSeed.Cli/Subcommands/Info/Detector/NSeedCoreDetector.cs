using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using System;
using System.Linq;

namespace NSeed.Cli.Subcommands.Info.Detector
{
    internal class NSeedCoreDetector : NSeedDetector
    {
        public NSeedCoreDetector(IDependencyGraphService dependencyGraphService, IFileSystemService fileSystemService)
            : base(dependencyGraphService, fileSystemService) { }

        protected override IOperationResponse<Project> DetectDependency(Project project)
        {
            switch (project)
            {
                case var proj when proj.Framework.IsDefined && proj.Framework.Dependencies.Contains(NSeed):
                    return OperationResponse<Project>.Success(project);
                case var proj when proj.Path.Exists():
                    var frameworkResponse = DependencyGraphService.GetProjectFramework(proj.Path);
                    return frameworkResponse.IsSuccessful && frameworkResponse.Payload!.Dependencies.Contains(NSeed)
                        ? OperationResponse<Project>.Success(project)
                        : OperationResponse<Project>.Error(frameworkResponse.Message);
                default:
                    return OperationResponse<Project>.Error(Resources.Info.Errors.NSeedProjectCouldNotBeFound);
            }
        }
    }
}
