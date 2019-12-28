using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using System;
using System.Linq;

namespace NSeed.Cli.Services
{
    internal class CoreNugetPackageDetector : INugetPackageDetector
    {
        protected IDependencyGraphService DependencyGraphService { get; }

        public CoreNugetPackageDetector(IDependencyGraphService dependencyGraphService)
        {
            DependencyGraphService = dependencyGraphService;
        }

        public IOperationResponse Detect(Project project, string nugetPackageName)
        {
            switch (project)
            {
                case var proj when proj.Framework.IsDefined
                    && proj.Framework.Dependencies.Any(d => d.Equals(nugetPackageName, StringComparison.OrdinalIgnoreCase)):
                    return OperationResponse<Project>.Success(project);
                case var proj when proj.Path.Exists():
                    var frameworkResponse = DependencyGraphService.GetProjectFramework(proj.Path);
                    return frameworkResponse.IsSuccessful && frameworkResponse.Payload!.Dependencies.Contains(nugetPackageName)
                        ? OperationResponse<Project>.Success(project)
                        : OperationResponse<Project>.Error(frameworkResponse.Message);
                default:
                    return OperationResponse<Project>.Error(Resources.Info.Errors.SeedBucketProjectCouldNotBeFound);
            }
        }
    }
}
